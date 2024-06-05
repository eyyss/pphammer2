using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCapsule : MonoBehaviour
{
    public float time = 50;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out LevelTimer levelTimer))
        {
            levelTimer.AddTime(time);
            Destroy(gameObject);
        }
    }
}
