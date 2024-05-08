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
        transform.DOScaleY(1, 0.4f).OnComplete(delegate
        {
            collider.isTrigger = true;
        });
    }

}
