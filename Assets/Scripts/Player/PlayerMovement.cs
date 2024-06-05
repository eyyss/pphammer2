using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayLength;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private CapsuleCollider2D collider;
    private float gravityScale = 2f;

    private float inputX;
    private float inputY;
    private float xVelocity = 1;

    private float moveSpeed;
    [SerializeField]private float walkSpeed = 4;
    [SerializeField] private float iceSpeed = 4;
    [SerializeField] private float mudSpeed = 4;

    [SerializeField] private float crouchSpeed = 2;
    [SerializeField] private Vector2 crouchColliderOffset;
    [SerializeField] private Vector2 crouchColliderScale;
    private Vector2 normalColliderOffset, normalColliderScale;

    
    [SerializeField] private float climbSpeed = 2.5f;
    [SerializeField]private float jumpHeight;

    [SerializeField] private bool isMove = true;
    [SerializeField] private bool isJump = true;
    [SerializeField] private bool isCrouching;
    [SerializeField] private bool isClimbing;

    [Header("Sliders")]
    public Slider jumpSlider;

    private Vector3 startPoint;

    public GroundType groundType;


    private float normalScaleX;


    

    void Start()
    {
        normalScaleX = transform.localScale.x;

        startPoint = transform.position;
        collider= GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = walkSpeed;
        gravityScale = rb.gravityScale;
        normalColliderOffset = collider.offset;
        normalColliderScale = collider.size;
    }

    void Update()
    {

        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (!isMove)
        {
            inputX = 0;
            inputY = 0;
        }

        animator.SetBool("IsCrouching",isCrouching);



        if (Input.GetAxisRaw("Down") != 0 && inputX == 0 && !isClimbing && isGrounded)
        {
            collider.size = crouchColliderScale;
            collider.offset = crouchColliderOffset;
            isCrouching = true;
            moveSpeed = crouchSpeed;
        }
        if (Input.GetAxisRaw("Down") == 0)
        {
            CheckUp();
        }



        RaycastHit2D hit = Physics2D.CircleCast(groundCheck.position, rayLength, Vector2.zero,0.1f, groundMask);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundMask);
        isGrounded = hit.collider != null;

        animator.SetBool("IsGrounded", isGrounded);
        if (Input.GetAxisRaw("Jump")!=0 && isGrounded && !isCrouching && isJump)
        {
            animator.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }

    }

    private void CheckUp()
    {
        StartCoroutine(RiseAfterDelay());
        IEnumerator RiseAfterDelay()
        {
            yield return new WaitForSeconds(0.1f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, groundMask);
            Debug.DrawRay(transform.position, Vector2.up*0.6f, hit.collider != null ? Color.blue : Color.yellow);
            if (hit.collider == null)
            {
                collider.size = normalColliderScale;
                collider.offset = normalColliderOffset;
                isCrouching = false;
                moveSpeed = walkSpeed;
                StopCoroutine(RiseAfterDelay());
            }
            else
            {
                StartCoroutine(RiseAfterDelay());
            }
        }
    }

    private void FixedUpdate()
    {

        if (!isClimbing)
        {
            switch (groundType)
            {
                case GroundType.normal:
                    rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);
                    break;
                case GroundType.ice:
                    rb.velocity = new Vector2(iceSpeed, rb.velocity.y);
                    break;
                case GroundType.mud:
                    rb.velocity = new Vector2(inputX * mudSpeed, rb.velocity.y);
                    break;
            }
        }
        else
        {
            if (isGrounded)
            {
                inputY = Mathf.Clamp(inputY,0,inputY*moveSpeed);
            }
            rb.velocity = new Vector2(inputX*moveSpeed, inputY* moveSpeed);
        }

        animator.SetBool("IsClimbing", isClimbing);

        if (!isClimbing)
        {
            float newInputX = inputX;
            if (groundType == GroundType.ice) newInputX = 0;

            animator.SetFloat("Speed", Mathf.Abs(newInputX));
        }
        else
        {
            animator.SetFloat("Speed", new Vector2(inputX,inputY).normalized.magnitude);
        }

        FlipCharacter();
    }

    private void FlipCharacter()
    {
        //if (inputX == 0) return;
        //transform.localScale = new Vector3(inputX, transform.localScale.y, transform.localScale.z);
        if (rb.velocity.x < 0) xVelocity = -normalScaleX;
        if(rb.velocity.x > 0) xVelocity = normalScaleX;
        transform.localScale = new Vector3(xVelocity, transform.localScale.y, transform.localScale.z);

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0;
            moveSpeed = climbSpeed;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = gravityScale;
            moveSpeed = walkSpeed;
        }
    }

    public void SetMove(bool state)
    {
        isMove = state;
        isJump = state; 
    }
    public Vector2 Veloctiy()
    {
        return rb.velocity;
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawRay(transform.position,Vector2.down*rayLength);
        Gizmos.DrawWireSphere(groundCheck.position, rayLength);
    }

    private Coroutine jumpHeightCoroutine,updateSliderValue;
    public void UseJumpHeightPotion()
    {

        if (jumpHeightCoroutine != null)
        {
            StopCoroutine(jumpHeightCoroutine);
            jumpHeight = jumpHeight / 2;
        }

        jumpHeightCoroutine = StartCoroutine(e());
        IEnumerator e()
        {
            jumpHeight = jumpHeight * 2;

            if (updateSliderValue !=null)
            {
                jumpSlider.value = jumpSlider.maxValue;
            }
            else
            {
                updateSliderValue = StartCoroutine(UpdateSliderValue());
            }

            yield return new WaitForSeconds(5f);
            jumpHeight = jumpHeight / 2;
            jumpHeightCoroutine = null;
        }

    }
    IEnumerator UpdateSliderValue()
    {
        jumpSlider.maxValue = 5f;
        jumpSlider.value = jumpSlider.maxValue;
        while (true)
        {
            if (jumpSlider.value >= jumpSlider.minValue)
            {
                yield return new WaitForSeconds(0.1f);
                jumpSlider.value = jumpSlider.value -= 0.1f;
            }
            else
            {
                StopCoroutine(UpdateSliderValue());
                updateSliderValue = null;
            }

        }

    }
    public void MoveToStartPoint()
    {
        transform.position = startPoint;
    }
    public void AddForce(Vector2 dir,ForceMode2D mode)
    {
        rb.AddForce(dir, mode);
    }
    public void ChangeGroundType(GroundType newType)
    {
        groundType = newType;
    }

    public void MoveToPosition(Vector3 pos)
    {
        rb.isKinematic = true;
        transform.position = pos;
        rb.isKinematic = false;
    }
}