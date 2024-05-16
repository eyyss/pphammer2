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

    private bool timerState = true;

    void Start()
    {
        health = GetComponent<PlayerHealth>();
        StartTimer();
    }
    public void StartTimer()
    {
        currentTime = levelTime;
    }
    public void SetTimerState(bool state)
    {
        timerState = state;
    }
    void Update()
    {
        if (timerState)
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
            health.Dead();
            Debug.Log("Zaman doldu!");
        }

    }
    
    public float CurrentTime()
    {
        return currentTime;
    }




}
