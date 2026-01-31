using UnityEngine;
using TMPro;
using System;
using GDC.Managers;

public class GamePlayManager : Singleton<GamePlayManager>
{
    [Header("Level Settings")]
    [SerializeField] private float levelTime = 60f;
    [SerializeField] private int targetPercent = 100;

    [Header("UI References")]
    [SerializeField] private Time_Bar timeBar;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Events")]
    public Action<int> OnGameWin;
    public Action<int> OnGameLose;
    public Action<float, float> OnTimeChanged;

    // Runtime Data
    private float m_currentTime;
    private int m_currentPercent;
    private bool m_isPlaying;

    private void Start()
    {
        StartLevel();
    }

    private void Update()
    {
        if (m_isPlaying)
        {
            HandleTimer();
        }
    }
    public void StartLevel()
    {
        SoundManager.Instance.PlayGamePlayBGM();
        m_currentTime = 0;
        OnTimeChanged?.Invoke(m_currentTime,levelTime);
        m_currentPercent = 0;
        m_isPlaying = true;

        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
        UpdateUI();
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

    private void GameWin()
    {
        m_isPlaying = false;
        Debug.Log("YOU WIN!");
        if (winPanel) winPanel.SetActive(true);
        OnGameWin?.Invoke(m_currentPercent);
    }

    private void GameLose()
    {
        m_isPlaying = false;
        Debug.Log("GAME OVER!");
        if (losePanel) losePanel.SetActive(true);
        OnGameLose?.Invoke(m_currentPercent);
    }

    private void UpdateUI()
    {
        if (scoreText) scoreText.text = $"{m_currentPercent}/{targetPercent}";
    }
}