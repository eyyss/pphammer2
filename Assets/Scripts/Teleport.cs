using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Teleport targetTeleport;
    public float speedMultipier = 4;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetTeleport == null) return;
        if(collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            Rigidbody2D rb = playerHealth.GetComponent<Rigidbody2D>();
            PlayerMovement movement = rb.GetComponent<PlayerMovement>();
            rb.gravityScale = 0;
            float duration = Vector2.Distance(playerHealth.transform.position, targetTeleport.transform.position)/ speedMultipier;
            
            playerHealth.spriteRenderer.color = Color.clear;
            playerHealth.transform.DOMove(targetTeleport.transform.position, duration).OnComplete(delegate
            {
                playerHealth.spriteRenderer.color = Color.white;
                rb.gravityScale = 2;
            });

            SoundManager.Instance.PlayOneShot("Teleport");
        }
    }
}
