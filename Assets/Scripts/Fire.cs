using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float damage;
    public bool isPlayerEnter;
    public string soundName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth health))
        {
            if(soundName != string.Empty)
            {
                SoundManager.Instance.PlayOneShot(soundName);
            }
            isPlayerEnter = true;
            StartCoroutine(SetDamage(health));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerInteraction health))
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
