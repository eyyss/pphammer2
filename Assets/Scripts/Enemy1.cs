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

    private void Start()
    {
        startPos = transform.position;
        targetPos = targetTransform.position;
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,targetPos, Time.deltaTime * moveDelta);
        float distance = Vector2.Distance(transform.position, targetPos);
        if (distance < 0.1f)
        {
            if(targetPos == startPos)
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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(damage);
        }
    }
}
