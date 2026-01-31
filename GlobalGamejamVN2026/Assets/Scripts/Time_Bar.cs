using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class Time_Bar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject timeBar;
    [SerializeField] private Image timeFill;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text timeText2;

    void OnDisable()
    {
        if (GamePlayManager.Instance == null)
            return;

        GamePlayManager.Instance.OnTimeChanged -= HandleTimeChanged;

    }

    void Start()
    {
        if (GamePlayManager.Instance == null)
        {
            return;
        }
        GamePlayManager.Instance.OnTimeChanged += HandleTimeChanged;
    }

    private void HandleTimeChanged(float currentTime, float maxTime)
    {
        float percent = Mathf.Clamp01(currentTime / maxTime);
        timeFill.fillAmount = percent;
        TimeSpan timeSpan = TimeSpan.FromSeconds(maxTime - currentTime);
        timeText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        timeText2.text = timeText.text;
    }
}
