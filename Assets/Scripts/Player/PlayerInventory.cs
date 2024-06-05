using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public List<SlotUI> slotList = new List<SlotUI>();
    public List<ItemData> itemDataList = new List<ItemData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public bool AddItem(ItemData itemData)
    {
        var slot = FindEmptySlotUI();
        if (slot != null)
        {
            itemDataList.Add(itemData);
            slot.Initialize(itemData);
            //Debug.Log(itemData.itemName);
            return true;
        }
        return false;
    }
    public void RemoveItem(ItemData itemData)
    {
        var slot = FindSlotUI(itemData);
        if(slot != null)
        {
            slot.RemoveItem();
            itemDataList.Remove(itemData);
        }
    }

    public SlotUI FindEmptySlotUI()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i].IsEmpty())return slotList[i];
        }

        return null;
    }
    public SlotUI FindSlotUI(ItemData itemData)
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            if (!slotList[i].IsEmpty())
            {
                if (slotList[i].itemData == itemData)
                    return slotList[i];
            }
        }
        return null;
    }
}
[Serializable]
public class ItemData
{
    public string itemName;
    public Sprite itemSprite;
    public ItemType itemType;
    public AudioClip useClip;
}


public enum ItemType
{
    BluePotion,YellowPotion,RedPotion,GreenPotion,GreenKey,RedKey,BlueKey
}

public interface ICollectable
{
    public void Collect();
}