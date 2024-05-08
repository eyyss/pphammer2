using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    public float maxHealth;
    public Slider healthSlider;
    public bool isDead;
    public int lifeCount = 3;
    public Text lifeText;


    public Animator animator;
    private Rigidbody2D rb;
    private Collider2D collider;
    private PlayerGhost ghost;
    private PlayerMovement playerMovement;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        ghost = GetComponent<PlayerGhost>();
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }
    public void TakeDamage(float damageValue)
    {
        if (ghost.isActiveGhostMode) return;

        health -= damageValue;
        healthSlider.value = health;
        if (health <= 0)
        {
            if (lifeCount<=0 && !isDead)
            {
                isDead = true;
                Dead();
            }
            if(lifeCount>0&&!isDead) { Respawn(); }
        }
    }

    private void Dead()
    {
        Debug.Log("Dead");
        playerMovement.SetMove(false);
    }

    private void Respawn()
    {
        animator.SetTrigger("Dead");
        RespawnAnimation();
        lifeCount--;
        lifeText.text = "Life: " + lifeCount;
        ResetHealth();
        healthSlider.value = health;
        Debug.Log("Respawn");
    }

    private void RespawnAnimation()
    {
        float normalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
        collider.enabled = false;
        playerMovement.SetMove(false);
        transform.DOMoveY(transform.position.y + 4f, 2f).OnComplete(delegate
        {
            collider.enabled = true;
            rb.gravityScale = normalGravityScale;
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
}
