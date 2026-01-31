using DG.Tweening;
using GDC.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public RectTransform startButtonRect, levelMenuButtonRect;

    private void Start()
    {
        SoundManager.Instance.SetMusicVolume(1);
    }
    public void ShowLevelMenu()
    {
        startButtonRect.DOAnchorPosX(-1980, 0.5f);
        levelMenuButtonRect.DOAnchorPosX(0, 0.5f);
    }

    public void ShowStartButton()
    {
        startButtonRect.DOAnchorPosX(0, 0.5f);
        levelMenuButtonRect.DOAnchorPosX(1980, 0.5f);
    }

    public void GoToScene(string sceneName)
    {
        GameManager.Instance.LoadSceneManually(sceneName, GDC.Enums.TransitionType.IN);
    }
}
