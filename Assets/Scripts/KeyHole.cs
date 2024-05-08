using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHole : MonoBehaviour
{
    public Door door;
    public ItemType keyType;
    public void OpenDoor()
    {
        if (keyType == door.keyType)
        {
            door.Open();
        }
    }
}
