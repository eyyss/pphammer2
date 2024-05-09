using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearth : MonoBehaviour,ICollectable
{
    private PlayerHealth health;
    private void Start()
    {
        health = FindObjectOfType<PlayerHealth>();
    }
    public void Collect()
    {
        Destroy(gameObject);
        health.AddLife(1);
    }


}
