using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool pause = false;
    public CanvasGroup pauseScreen;
    public int needTreasureCount;
    public string nextLevelCode = "00000";
    private int treasure;
    private int score;
    public CanvasGroup levelEndScreen;
    public Text treasureText;
    public Text timeLeftText;
    public Text scoreText;
    public Text levelEndScoreText;
    public Text codeForNextLevelText;
    public Text pressSpaceToContinueText;


    private LevelTimer levelTimer;

    private PlayerHealth health;
    private void Start()
    {
        Transition.Instance?.FadeOut();
        health = GetComponent<PlayerHealth>();
        levelTimer = GetComponent<LevelTimer>();

        treasure = needTreasureCount;
        treasureText.text = treasure.ToString();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            pause = !pause;
            Time.timeScale = pause ? 0 : 1;
            pauseScreen.alpha = pause ? 1 : 0;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            health.Dead();
        }
    }
    public void OpenLevelEndScreen()
    {
        timeLeftText.text = "TIME LEFT: " + ((int)levelTimer.CurrentTime()).ToString();
        levelEndScoreText.text = "SCORE: " + score.ToString();
        codeForNextLevelText.text = "CODE FOR NEXT LEVEL IS: "+ nextLevelCode;
        health.GetComponent<PlayerMovement>().SetMove(false);

        StartCoroutine(e());
        IEnumerator e()
        {
            yield return new WaitForSeconds(5);
            Transition.Instance?.FadeIn();
            yield return new WaitForSeconds(1);
            Transition.Instance?.FadeOut();
            levelEndScreen.alpha = 1;


            int time = (int)levelTimer.CurrentTime();

            DOTween.To(() => time, x => time = x, 0, 5).OnUpdate(delegate
            {
                timeLeftText.text = "TIME LEFT: " + time.ToString();
            });

            int tempScore = score;
            DOTween.To(() => score, x => score = x, tempScore + time * 10, 5).OnUpdate(delegate
            {
                levelEndScoreText.text = "SCORE: " + score.ToString();
            }).OnComplete(delegate
            {
                HighScoreCalculate(score);
                Debug.Log(score);
            });

            yield return new WaitForSeconds(3f);
            pressSpaceToContinueText.gameObject.SetActive(true);
            yield return new WaitUntil(delegate { return Input.GetKeyDown(KeyCode.Space); });
            Transition.Instance?.FadeIn();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(0);
            Debug.Log("Level Geçti Menuye Donuyor");
        }
    }
    public void HighScoreCalculate(int _score)
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            int savedScore = PlayerPrefs.GetInt("Score");
            if (savedScore < _score) PlayerPrefs.SetInt("Score", _score);
        }
        else
        {
            PlayerPrefs.SetInt("Score", _score);
        }
    }
    public void AddTreasure()
    {
        score += 50;
        scoreText.text = "SCORE: " + score.ToString();
        treasure--;
        treasureText.text = treasure.ToString();
        if(treasure<= needTreasureCount)
        {
            Debug.Log("Kapý açýldý");

            FindObjectOfType<LevelEndDoor>().Open();
        }
    }
    
}

