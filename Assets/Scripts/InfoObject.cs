using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoObject : MonoBehaviour
{
    public string enterInfo,exitInfo;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerInteraction playerInteraction))
        {
            playerInteraction.infoText.UpdateText(enterInfo);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerInteraction playerInteraction))
        {
            playerInteraction.infoText.UpdateText(exitInfo);
            Destroy(gameObject);
        }
    }
}
