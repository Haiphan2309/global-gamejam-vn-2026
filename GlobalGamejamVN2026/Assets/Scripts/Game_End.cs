using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using UnityEngine.SocialPlatforms.Impl;
using DG.Tweening;

public class Game_End : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text resultText_shadow;
    [SerializeField] private GameObject hanabi;
    [SerializeField] private GameObject button;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreText2;
    [SerializeField] private float timeSet = 5f;
    [SerializeField] private RectTransform panel;
    private bool isWin = false;
    

    void OnEnable()
    {
        panel.anchoredPosition = new Vector2(0, -10f);
        resultText.gameObject.SetActive(false);
        resultText_shadow.gameObject.SetActive(false);
        hanabi.SetActive(false);
        button.SetActive(false);
        if (GamePlayManager.Instance == null)
            return;

        GamePlayManager.Instance.OnGameWin += ShowScoreWin;
        GamePlayManager.Instance.OnGameLose += ShowScoreLose;


    }

    void OnDisable()
    {
        resultText.gameObject.SetActive(false);
        resultText_shadow.gameObject.SetActive(false);
        if (GamePlayManager.Instance == null)
            return;

        GamePlayManager.Instance.OnGameWin -= ShowScoreWin;
        GamePlayManager.Instance.OnGameLose -= ShowScoreLose;
    }

    public void ShowScoreWin(int scoreValue)
    {
        isWin = true;
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
        panel.DOAnchorPosY(0, 1f).SetEase(Ease.OutBack);
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

        if(isWin)
        {
            FaceController.Instance.Happy();
            hanabi.SetActive(true);
            button.SetActive(true);
        }
        else
        {
            FaceController.Instance.Angry();
            button.SetActive(true);
            Transform children = button.transform.GetChild(2);
            children.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);

        resultText.gameObject.SetActive(true);
        resultText_shadow.gameObject.SetActive(true);


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
