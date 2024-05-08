using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public SpriteRenderer triangle;
    public bool isPlayerEnter = false;
    public float damage;
    private void Start()
    {
        triangle.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth health))
        {
            isPlayerEnter = true;
            StartCoroutine(SetDamage(health));
            triangle.gameObject.SetActive(true);
            triangle.transform.DOMove(transform.position, 0.2f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth health))
        {
            isPlayerEnter = false;
        }
    }

    private IEnumerator SetDamage(PlayerHealth health)
    {
        while (isPlayerEnter)
        {
            yield return new WaitForSeconds(0.1f);
            health.TakeDamage(damage);
        }
    }

}
