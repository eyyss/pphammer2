using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public Image image;
    public ItemData itemData;
    public void Initialize(ItemData _itemData)
    {
        itemData = _itemData;
        image.sprite = itemData.itemSprite;
    }

    public void RemoveItem()
    {
        image.sprite = null;
        itemData = null;
    }

    public bool IsEmpty()
    {
        return image.sprite == null;
    }

}
