using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using UnityEngine.SocialPlatforms.Impl;
using DG.Tweening;
using GDC.Managers;

public class Game_End : Singleton<Game_End>
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

    bool isActiveAgain = false;
    int scoreValue = 0;

    void OnEnable()
    {
        panel.anchoredPosition = new Vector2(0, -1000f);
        resultText.gameObject.SetActive(false);
        resultText_shadow.gameObject.SetActive(false);
        hanabi.SetActive(false);
        button.SetActive(false);
    }

    void OnDisable()
    {
        resultText.gameObject.SetActive(false);
        resultText_shadow.gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (isActiveAgain)
        {
            StartCoroutine(ScoreRoutine(scoreValue));
            isActiveAgain = false;
        }
    }
    public void ShowScoreWin(int scoreValue)
    {
        FaceController.Instance.Happy();
        isWin = true;
        Debug.Log("Show Score Win called with score: " + scoreValue);
        resultText.text = "YOU WIN!";
        resultText_shadow.text = "YOU WIN!";

        StopAllCoroutines();
        panel.gameObject.SetActive(true);
        panel.DOAnchorPosY(0, 1f).SetEase(Ease.OutBack).SetDelay(2f).OnComplete(()=>
        {
            SoundManager.Instance.SetMusicVolume(0);
            SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_WIN);
            });
        isActiveAgain = true;
        this.scoreValue = scoreValue;
    }

    public void ShowScoreLose(int scoreValue)
    {
        FaceController.Instance.Angry();
        Debug.Log("Show Score Lose called with score: " + scoreValue);
        resultText.text = "TRY AGAIN!";
        resultText_shadow.text = "TRY AGAIN!";
        
        StopAllCoroutines();
        panel.gameObject.SetActive(true);
        panel.DOAnchorPosY(0, 1f).SetEase(Ease.OutBack).SetDelay(2f).OnComplete(() => {
            SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_LOSE);
            SoundManager.Instance.SetMusicVolume(0);
        });
        isActiveAgain = true;
        this.scoreValue = scoreValue;
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
            
            hanabi.SetActive(true);
            button.SetActive(true);
        }
        else
        {
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
        GameManager.Instance.LoadSceneManually("Level_" + (SceneManager.GetActiveScene().buildIndex - 1).ToString(), GDC.Enums.TransitionType.IN);
    }
    public void GoToMainMenu()
    {
        GameManager.Instance.LoadSceneManually("MainMenu", GDC.Enums.TransitionType.IN);
    }
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            GameManager.Instance.LoadSceneManually("MainMenu", GDC.Enums.TransitionType.IN);
            return;
        }
        GameManager.Instance.LoadSceneManually("Level_" + (SceneManager.GetActiveScene().buildIndex - 1).ToString(), GDC.Enums.TransitionType.IN);
    }

}
