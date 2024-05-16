using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public GroundType enterType,exitType;
    private PlayerMovement movement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out movement))
        {
            movement.ChangeGroundType(enterType);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out movement))
        {
            movement.ChangeGroundType(exitType);
        }
    }
}
public enum GroundType
{
    normal,ice,mud
}