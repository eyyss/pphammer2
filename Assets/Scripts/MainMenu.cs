using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup loadScreen;
    public InputField levelCodeField;
    public Text highScoreText;
    public List<LevelData> levelDataList;
    
    private void Start()
    {
        Transition.Instance?.FadeOut();
        UpdateHighScoreText();
    }
    private void UpdateHighScoreText()
    {
        highScoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("Score", 0);
    }
    public void LoadScene(int buildIndex)
    {
        foreach (var data in levelDataList)
        {
            if(levelCodeField.text == data.levelCode.ToString())
            {
                buildIndex = data.levelBuildIndex;
            }
        }

        StartCoroutine(s());
        IEnumerator s()
        {
            Transition.Instance?.FadeIn();
            yield return new WaitForSeconds(2);
            Transition.Instance?.FadeOut();
            loadScreen.alpha = 1;
            yield return new WaitForSeconds(1);
            Transition.Instance?.FadeIn();
            yield return new WaitForSeconds(1);
            var operation = SceneManager.LoadSceneAsync(buildIndex);
            yield return new WaitUntil(delegate { return operation.isDone; });
        }
    }
    public void UpdateLevelCodeText(string str)
    {
        str = str.ToUpper();
        levelCodeField.text = str;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    [Serializable]
    public class LevelData
    {
        public int levelCode;
        public int levelBuildIndex;
    }
}
