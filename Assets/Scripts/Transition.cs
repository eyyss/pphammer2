using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public static Transition Instance;
    public CanvasGroup canvasGroup;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        canvasGroup.alpha = 1;
    }
    public void FadeIn()
    {
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, 1).OnComplete(delegate
        {

        });
    }
    public void FadeOut()
    {
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, 1).OnComplete(delegate
        {

        });
    }
}
