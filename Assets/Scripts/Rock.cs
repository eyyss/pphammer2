using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public Transform deadTransform;

    private void Update()
    {
        var hit = Physics2D.BoxCast(deadTransform.position, deadTransform.localScale, 0, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out PlayerHealth health))
            {
                health.TakeDamage(1000);
            }

        }

    }
    private void OnDrawGizmos()
    {
        if(!deadTransform) return;
        Gizmos.DrawWireCube(deadTransform.position, deadTransform.localScale);
    }

}
