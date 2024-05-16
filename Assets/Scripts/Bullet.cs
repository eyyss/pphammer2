using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 20;
    private Vector3 moveDirection;
    public float damage = 1;
    public void Initialize(Vector3 newMoveDirection)
    {
        moveDirection = newMoveDirection;
    }
    public void Update()
    {
        transform.Translate(moveDirection*Time.deltaTime*moveSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
