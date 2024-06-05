using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    public SpriteRenderer spriteRender;
    public float duration;
    public bool isActiveGhostMode;
    
    public void OpenGhostMode()
    {
        spriteRender.color = Color.gray;
        Debug.Log("test");
    }
    public void CloseGhostMode()
    {
        spriteRender.color = Color.white;
    }

    public IEnumerator GhostModeAction()
    {
        OpenGhostMode();
        isActiveGhostMode = true;
        yield return new WaitForSeconds(duration);
        isActiveGhostMode = false;
        CloseGhostMode();
    }

}
