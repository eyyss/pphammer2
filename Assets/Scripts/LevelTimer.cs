using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    public int levelTime = 60; 
    private float currentTime;
    public Text levelTimerText;
    private bool complate = false;
    private PlayerHealth health;
    public CanvasGroup deadScreeen;
    public CanvasGroup loadScreen;
    public Slider levelContinueSlider;

    private bool second10;
    private float second10Timer = 10;
    void Start()
    {
        health = GetComponent<PlayerHealth>();
        levelContinueSlider.maxValue = second10Timer;
        StartTimer();
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        levelTimerText.text = "Time: "+((int)currentTime).ToString();
        if (currentTime<0)
        {
            currentTime = 0;
        }

        if (currentTime <= 0 && !complate)
        {
            complate = true;
            currentTime = 0;
            StartCoroutine(RestartScene());
            Debug.Log("Zaman doldu!");
        }

        if (second10)
        {
            second10Timer -= Time.deltaTime;
            levelContinueSlider.value = second10Timer;
        }
        
    }

    IEnumerator RestartScene()
    {
        health.Dead();
        yield return new WaitForSeconds(2);
        deadScreeen.alpha = 1;
        StartCoroutine(Wait10Seconds());
        yield return new WaitUntil(delegate {
            return Input.GetKeyDown(KeyCode.Space);
        });

        loadScreen.alpha = 1;
        yield return new WaitForSeconds(4);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Wait10Seconds()
    {
        second10 = true;
        yield return new WaitForSeconds(10);
        second10 = false;
        SceneManager.LoadScene("Menu");
    }

    public void StartTimer()
    {
        currentTime = levelTime;
    }

}
