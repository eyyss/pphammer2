using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    public float maxHealth;
    public Slider healthSlider;
    public bool isDead;
    public bool isRespawn;
    public int lifeCount = 3;
    public Text lifeText;


    public Animator animator;
    private Rigidbody2D rb;
    private Collider2D collider;
    private PlayerGhost ghost;
    private PlayerMovement playerMovement;
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.white;

    public CanvasGroup deadScreeen;
    public CanvasGroup loadScreen;
    public Slider levelContinueSlider;
    private float second10Timer = 10;
    private bool second10;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        ghost = GetComponent<PlayerGhost>();
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        lifeText.text = "Life: " + lifeCount;

        levelContinueSlider.maxValue = second10Timer;
    }

    public void TakeDamage(float damageValue)
    {
        if (ghost.isActiveGhostMode) return;

        health -= damageValue;
        healthSlider.value = health;
        StopCoroutine(HitAnimation());
        StartCoroutine(HitAnimation());
        if (health <= 0)
        {
            if(lifeCount>=1&&!isDead) { Respawn(); }
        }
    }
    IEnumerator HitAnimation()
    {
        Color tempColor = spriteRenderer.color;
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = tempColor;
    }

    public void Dead()
    {
        Debug.Log("Dead");
        lifeCount=0;
        lifeText.text = "Life: " + lifeCount;
        health -= 0;
        healthSlider.value = health;
        DeadAnimation();
    }
    private void DeadAnimation()
    {
        animator.SetTrigger("Dead");
        float normalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
        collider.enabled = false;
        playerMovement.SetMove(false);
        transform.DOMoveY(transform.position.y + 4f, 2f).OnComplete(delegate
        {
            //olum sesini cal
            StartCoroutine(GoDeadScreen());
        });
    }
    IEnumerator GoDeadScreen()
    {
        Transition.Instance?.FadeIn();
        yield return new WaitForSeconds(2);
        Transition.Instance?.FadeOut();
        deadScreeen.alpha = 1;
        StartCoroutine(Wait10Seconds());
        yield return new WaitUntil(delegate {
            return Input.GetKeyDown(KeyCode.Space);
        });

        Transition.Instance?.FadeIn();
        yield return new WaitForSeconds(1);
        loadScreen.alpha = 1;
        yield return new WaitForSeconds(4);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator Wait10Seconds()
    {
        second10 = true;
        yield return new WaitForSeconds(10);
        second10 = false;
        Transition.Instance?.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }

    public void Respawn()
    {
 
        lifeCount--;
        lifeText.text = "Life: " + lifeCount;
        if (lifeCount < 1 && !isDead)
        {
            isDead = true;
            Dead();
            return;
        }

        animator.SetTrigger("Dead");
        RespawnAnimation();
        ResetHealth();
        healthSlider.value = health;
        Debug.Log("Respawn");
    }

    private void RespawnAnimation()
    {
        float normalGravityScale = rb.gravityScale;
        isRespawn = true;
        rb.gravityScale = 0;
        collider.enabled = false;
        playerMovement.SetMove(false);
        transform.DOMoveY(transform.position.y + 4f, 2f).OnComplete(delegate
        {
            collider.enabled = true;
            rb.gravityScale = normalGravityScale;
            isRespawn = false;
            playerMovement.SetMove(true);
            playerMovement.MoveToStartPoint();
            animator.SetTrigger("Respawn");
        });
    }

    public void ResetHealth()
    {
        health = maxHealth;
        healthSlider.value = health;
    }
    public void AddLife(int count)
    {
        lifeCount += count;
        lifeText.text = "Life: " + lifeCount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12) && !isRespawn && !isDead)
        {
            Respawn();
        }


        if (second10)
        {
            second10Timer -= Time.deltaTime;
            levelContinueSlider.value = second10Timer;
        }
    }
}
