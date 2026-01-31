using UnityEngine;
using TMPro;
using System;
using GDC.Managers;

public class GamePlayManager : Singleton<GamePlayManager>
{
    [Header("Level Settings")]
    [SerializeField] private float levelTime = 60f;
    [SerializeField] private int targetPercent = 100;
    [SerializeField] private UITargetBanner targetPanel;

    [Header("UI References")]
    [SerializeField] private Time_Bar timeBar;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject endPanel;

    [Header("Events")]
    public Action<int> OnGameWin;
    public Action<int> OnGameLose;
    public Action<float, float> OnTimeChanged;

    // Runtime Data
    private float m_currentTime;
    private int m_currentPercent;
    private bool m_isPlaying;

    public int currentLevel;

    public bool isNeedShowTutorial;
    public bool isShowingTutorial;

    private void Start()
    {
        SoundManager.Instance.SetMusicVolume(1);
        SoundManager.Instance.PlayGamePlayBGM();
    }

    private void Update()
    {
        if (m_isPlaying)
        {
            HandleTimer();
        }

        if (isShowingTutorial && PopupManager.Instance.IsPopupShowing())
        {
            targetPanel.SetupTargetPanel(targetPercent);
            m_currentTime = 0;
            OnTimeChanged?.Invoke(m_currentTime, levelTime);
            m_currentPercent = 0;
            m_isPlaying = true;

            UpdateUI();
        }
    }
    public void StartLevel()
    {
        if (isNeedShowTutorial)
        {
            if (currentLevel == 1)
            {
                isShowingTutorial = true;
                PopupManager.Instance.ShowTutorial1();
            }
            else if (currentLevel == 2)
            {
                isShowingTutorial = true;
                PopupManager.Instance.ShowTutorial2();
            }
            else if (currentLevel == 3)
            {
                isShowingTutorial = true;
                PopupManager.Instance.ShowTutorial3();
            }
        }
        else
        {

            targetPanel.SetupTargetPanel(targetPercent);
            m_currentTime = 0;
            OnTimeChanged?.Invoke(m_currentTime, levelTime);
            m_currentPercent = 0;
            m_isPlaying = true;

            UpdateUI();
        }
    }

    private void HandleTimer()
    {
        m_currentTime += Time.deltaTime;
        OnTimeChanged?.Invoke(m_currentTime,levelTime);
        if (m_currentTime >= levelTime)
        {
            m_currentTime = levelTime;
            CheckEndGameCondition();
        }
    }

    private void CheckEndGameCondition()
    {
        m_currentPercent = FaceController.Instance.CalculateResult();
        Debug.Log($"You got {m_currentPercent} percentage");
        if (m_currentPercent >= targetPercent)
        {
            GameWin();
        }
        else
        {
            GameLose();
        }
    }

    public void GameWin()
    {
        m_isPlaying = false;
        Debug.Log("YOU WIN!");
        if (endPanel) endPanel.SetActive(true);
        Game_End.Instance.ShowScoreWin(m_currentPercent);
    }

    public void GameLose()
    {
        m_isPlaying = false;
        Debug.Log("GAME OVER!");
        if (endPanel) endPanel.SetActive(true);
        else
        {
            Debug.Log("A");
        }
        m_currentPercent = FaceController.Instance.CalculateResult();
        Game_End.Instance.ShowScoreLose(m_currentPercent);
    }

    private void UpdateUI()
    {
        if (scoreText) scoreText.text = $"{m_currentPercent}/{targetPercent}";
    }
}