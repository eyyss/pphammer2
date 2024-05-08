using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private PlayerGhost playerGhost;
    private PlayerOil playerOil;

    private KeyHole keyHole;


    private void Start()
    {
        playerMovement= GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        playerGhost = GetComponent<PlayerGhost>();
        playerOil = GetComponent<PlayerOil>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UseItem(4);
        }
    }

    public void UseItem(int index)
    {
        PlayerInventory inventory = PlayerInventory.Instance;
        if (!inventory.slotList[index].IsEmpty())
        {
            bool use = Use(inventory.slotList[index].itemData.itemType);
            Debug.Log(inventory.slotList[index].itemData.itemType);
            if (use)
                inventory.RemoveItem(inventory.slotList[index].itemData);
        }
    }
    public bool Use(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.BluePotion:
                playerMovement.UseJumpHeightPotion();
                break;
            case ItemType.YellowPotion:
                StartCoroutine(playerGhost.GhostModeAction());
                break;
            case ItemType.RedPotion:
                playerHealth.ResetHealth();
                break;
            case ItemType.GreenPotion:
                StartCoroutine( playerOil.OilAction());
                break;
            case ItemType.GreenKey:
                if (keyHole != null)
                {
                    keyHole.OpenDoor();
                }
                else return false;
                    
                break;
        }
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out KeyHole _keyHole))
        {
            keyHole = _keyHole;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out KeyHole _keyHole))
        {
            keyHole = null;
        }
    }
}
