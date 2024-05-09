using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.white;

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
        transform.DOMoveY(transform.position.y + 4f, 2f);
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
    public void AddLife(int count)
    {
        lifeCount += count;
        lifeText.text = "Life: " + lifeCount;
    }
}
