using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RockTrigger : MonoBehaviour
{
    private Rock rock;
    private Collider2D collider;
    public float strength;
    public int vibrato;
    [Tooltip("Pozisyonlara göre tam düþüceði pozisyonu bu deðiþkene yazmalýsýn")]
    public Vector3 fallPosition;
    public float fallDuration = 1;
    private void Start()
    {
        collider = GetComponent<Collider2D>();
        rock = transform.parent.GetComponent<Rock>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerMovement movement))
        {
            collider.enabled = false;
            StartCoroutine(Fall());
        }
    }
    private IEnumerator Fall()
    {
        rock.transform.DOShakePosition(0.55f, strength, vibrato);
        yield return new WaitForSeconds(0.6f);
        rock.transform.DOMove(fallPosition, fallDuration);
    }
}
