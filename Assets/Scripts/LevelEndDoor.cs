using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndDoor : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out GameManager manager))
        {
            animator.SetTrigger("Open");
            FindObjectOfType<LevelTimer>().SetTimerState(false);
            manager.OpenLevelEndScreen();
        }
    }
    public void Open()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
       
    }
}
