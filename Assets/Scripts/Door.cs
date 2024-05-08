using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Collider2D collider;
    public SpriteRenderer renderer;
    public ItemType keyType;
    public void Open()
    {
        collider.enabled = false;
        transform.DOScaleY(1, 0.4f);
    }

}
