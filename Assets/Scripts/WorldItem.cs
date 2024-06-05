using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour, ICollectable
{
    public ItemData itemData;
    public void Collect()
    {
        if (PlayerInventory.Instance.AddItem(itemData))
        {
            Destroy(gameObject);
            SoundManager.Instance.PlayOneShot("Collect");
        }
    }

}

