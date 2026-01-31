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
    [SerializeField] private float timeSet;
    public void Start()
    {
        timeBar.SetActive(true);
        StartCoroutine(TimeRoutine());
    }

    // Update is called once per frame
    IEnumerator TimeRoutine()
    {
        float time=0;
        while(time < timeSet)
        {
            time += Time.deltaTime;
            float percent = Mathf.Clamp01(time / timeSet);
            timeFill.fillAmount = percent;
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeSet - time);
            timeText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            timeText2.text = timeText.text;
            yield return null;  
        }
    }
}
