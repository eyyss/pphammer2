using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Teleport targetTeleport;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetTeleport == null) return;
        if(collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.transform.position = targetTeleport.transform.position;
        }
    }
}
