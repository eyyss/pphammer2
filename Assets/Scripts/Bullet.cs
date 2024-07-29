using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 20;
    private Vector3 moveDirection;
    public float damage = 1;
    private SpriteRenderer renderer;


    public void Initialize(Vector3 newMoveDirection,Color color)
    {
        
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = color;
        moveDirection = newMoveDirection;

    }
    public void Update()
    {
        transform.Translate(moveDirection * Time.deltaTime * moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
        Debug.Log("Test");
    }
}
