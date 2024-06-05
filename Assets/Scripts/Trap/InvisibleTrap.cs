using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleTrap : MonoBehaviour
{
    public bool startTrap;
    public bool targetStart;
    public Transform targetTransform;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = targetStart ? Color.red : Color.green;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetStart)
        {
            if(collision.TryGetComponent(out PlayerMovement movement))
            {
                Vector3 newPosition = targetTransform.position;
                newPosition.y+= 0.5f;
                movement.MoveToPosition(newPosition);
            }
        }
    }
}
