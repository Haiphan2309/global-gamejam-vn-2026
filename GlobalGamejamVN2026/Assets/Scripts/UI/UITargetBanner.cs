using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITargetBanner : MonoBehaviour
{
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private TMP_Text targetTextShadow;

    public void SetupTargetPanel(int targetPercentage)
    {
        targetText.gameObject.SetActive(true);
        targetTextShadow.gameObject.SetActive(true);
        targetText.text = $"COVERAGE TARGET: \n {targetPercentage}%";
        targetTextShadow.text = targetText.text;
    }
}
