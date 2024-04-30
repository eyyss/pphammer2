using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerDig : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;

    [SerializeField] private float digDistance;

    [SerializeField] private GameObject destroyableObjectPrefab;
    [SerializeField] private float destroyableObjectSpawnTime;
    [SerializeField] private LayerMask layerMask;

    public float normalDigTime = 1, fastDigTime = 0.5f;
    private float digTime;


    private bool dig;


    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        digTime = normalDigTime;
    }
    public void ChangeDigTime(float value)
    {
        digTime = value;
    }
    void Update()
    {

        

        Vector3 dir = transform.right;
        if (transform.localScale.x > 0) dir = transform.right;
        if (transform.localScale.x < 0) dir = -transform.right;

        Vector3 pos = transform.position+ dir * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, digDistance);
        Debug.DrawRay(pos, Vector2.down* digDistance);

        bool isGrounded = animator.GetBool("IsGrounded");

        if (!CheckDir() && isGrounded)
        {
            if (hit.collider != null && hit.collider.CompareTag(Const.DestroyableObjectTag))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
                {
                    dig = true;
                    playerMovement.SetMove(false);
                    animator.SetTrigger("Dig");
                    StartCoroutine(ResetDig(hit));
                }
            }
        }
        



    }
    private IEnumerator ResetDig(RaycastHit2D hit)
    {
        yield return new WaitForSeconds(digTime);
        if (hit.collider.CompareTag(Const.DestroyableObjectTag))
        {
            Vector3 pos = hit.collider.transform.position;
            Destroy(hit.collider.gameObject);
            StartCoroutine(CreateDeletedObject(pos));
        }
        dig = false;
        playerMovement.SetMove(true);
    }

    private IEnumerator CreateDeletedObject(Vector3 pos)
    {
        yield return new WaitForSeconds(destroyableObjectSpawnTime);
        Instantiate(destroyableObjectPrefab, pos, Quaternion.identity);
    }

    private bool CheckDir()
    {
        Vector3 pos = transform.position - Vector3.up * 0.5f;
        Vector3 dir = transform.right;
        if (transform.localScale.x > 0) dir = transform.right;
        if (transform.localScale.x < 0) dir = -transform.right;

        RaycastHit2D hit = Physics2D.Raycast(pos, dir, 2, layerMask);
        Debug.DrawRay(pos, dir *2 ,hit.collider != null ? Color.red : Color.green  );
        if (hit.collider!=null && hit.collider.CompareTag(Const.DestroyableObjectTag))
        {
            return true;
        }
        return false;
    }

}
