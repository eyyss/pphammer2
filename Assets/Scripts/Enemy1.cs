using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float moveDelta = 2f;
    public Transform targetTransform;
    private Vector3 startPos;
    private Vector3 targetPos;
    public int damage;
    public int pushForce = 1;
    private Vector3 scale;

    public bool fly;
    public float flyRate;
    private Vector3 offset = new Vector3(0,2,0);

    private void Start()
    {
        scale = transform.localScale;
        startPos = transform.position;
        targetPos = targetTransform.position;
        StartCoroutine(ChangeOffsetY());
    }

    IEnumerator ChangeOffsetY()
    {
        yield return new WaitForSeconds(flyRate);
        if (offset.y > 0) offset.y = -1;
        else if (offset.y < 0) offset.y = 1;
        
        StartCoroutine(ChangeOffsetY());
    }

    private void Update()
    {



        transform.position = Vector2.MoveTowards(transform.position, targetPos + offset, Time.deltaTime * moveDelta);
        float distance = Vector2.Distance(transform.position, targetPos);
        if (distance < 0.1f)
        {
            if (targetPos == startPos)
            {
                targetPos = targetTransform.position;
                return;
            }
            if (targetPos == targetTransform.position)
            {
                targetPos = startPos;
                return;
            }
        }

        Flip();

    }

    private void Flip()
    {
        Vector3 direction = transform.position - targetPos;
        if (direction.normalized.x < 0)
        {
            transform.localScale = scale;
        }
        else
        {
            Vector3 newScale = scale;
            newScale.x = -newScale.x;
            transform.localScale = newScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(damage);
        }
    }
}
