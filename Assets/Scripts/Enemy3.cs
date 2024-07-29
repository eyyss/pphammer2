using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public LayerMask targetMask;
    public float damage = 1;
    public float fireRate = 1;
    private float fireTimer;
    private SpriteRenderer renderer;
    public GameObject bulletPrefab;
    private void Start()
    {
        renderer =GetComponent<SpriteRenderer>();
        fireTimer = fireRate;
    }
    private void Update()
    {
        fireTimer += Time.deltaTime;


        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 999f, targetMask);
        if (hit.collider != null)
        {
            if(hit.collider.TryGetComponent(out PlayerHealth health))
            {
                if (fireTimer>fireRate)
                {
                    fireTimer = 0;
                    var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    bullet.GetComponent<Bullet>().Initialize(transform.right,renderer.color);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.right * 999f);
    }
}
