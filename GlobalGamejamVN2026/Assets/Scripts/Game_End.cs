using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using UnityEngine.SocialPlatforms.Impl;

public class Game_End : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text resultText_shadow;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreText2;
    [SerializeField] private float timeSet = 5f;
    

    void OnEnable()
    {
        if (GamePlayManager.Instance == null)
            return;

        GamePlayManager.Instance.OnGameWin += ShowScoreWin;
        GamePlayManager.Instance.OnGameLose += ShowScoreLose;


    }

    void OnDisable()
    {
        if (GamePlayManager.Instance == null)
            return;

        GamePlayManager.Instance.OnGameWin -= ShowScoreWin;
        GamePlayManager.Instance.OnGameLose -= ShowScoreLose;
    }

    public void ShowScoreWin(int scoreValue)
    {
        Debug.Log("Show Score Win called with score: " + scoreValue);
        resultText.text = "YOU WIN!";
        resultText_shadow.text = "YOU WIN!";

        StopAllCoroutines();
        StartCoroutine(ScoreRoutine(scoreValue));
    }

    private void ShowScoreLose(int scoreValue)
    {
        Debug.Log("Show Score Lose called with score: " + scoreValue);
        resultText.text = "TRY AGAIN!";
        resultText_shadow.text = "TRY AGAIN!";
        StopAllCoroutines();
        StartCoroutine(ScoreRoutine(scoreValue));
    }

    IEnumerator ScoreRoutine(int scoreValue)
    {
        int countFPS = 30;  // số lần cập nhật trong 1 giây
        float time = 0f;
        WaitForSeconds wait = new WaitForSeconds(1f / countFPS);

        while (time < timeSet)
        {
            int score = Random.Range(0, 99);
            scoreText.text = score.ToString("D2");
            scoreText2.text = score.ToString("D2");

            time += 1f / countFPS;   // tăng đúng bằng khoảng chờ
            yield return wait;
        }

        // Hiện số cuối cùng chính xác
        scoreText.text = scoreValue.ToString("D2");
        scoreText2.text = scoreValue.ToString("D2");

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
