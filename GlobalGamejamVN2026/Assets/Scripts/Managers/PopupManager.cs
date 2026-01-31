using DG.Tweening;
using GDC.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [Header("Popup Manager Element")]
    [SerializeField] private Transform canvasTrans;
    [SerializeField] private Image blackBgPrefab;
    private Stack<Image> blackBgStack = new Stack<Image>();
    private Stack<UIBasePopup> popupStack;
    [SerializeField] private TMP_Text touchOutsideText;

    [Header("Popup prefab")]
    [SerializeField] private UISetting uiSettingPrefab;
    [SerializeField] private UIBasePopup uiTutorial1, uiTutorial2, uiTutorial3;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        popupStack = new Stack<UIBasePopup>();
    }
    void Update()
    {
        if (popupStack.Count > 0 && Input.GetMouseButtonDown(0))
        {
            GameObject clickedUIObject = UIUtils.GetUIObjectUnderPointer();

            if (clickedUIObject != null && clickedUIObject.CompareTag("UIBlack"))
            {
                TryHideCurrentPopup();
            }
        }
    }

    public UIBasePopup GetCurrentPopup()
    {
        if (popupStack.Count > 0)
        {
            return null;
        }

        return popupStack.Peek();
    }
    private void TryHideCurrentPopup() //Hide by touch outside the popup
    {
        if (popupStack.Count == 0)
        {
            return;
        }

        if (!popupStack.Peek().IsHideWhenTouchOutside)
        {
            return;
        }

        popupStack.Peek().Hide();
    }

    public bool IsPopupShowing()
    {
        return popupStack.Count > 0;
    }

    public void ClosePopup()
    {
        if (popupStack.Count > 0)
        {
            HideBlackBg();
            popupStack.Pop();
            if (popupStack.Count > 0)
            {
                if (popupStack.Peek().IsHideWhenTouchOutside)
                {
                    touchOutsideText.gameObject.SetActive(true);
                    touchOutsideText.transform.SetAsLastSibling();           
                }
                else
                {
                    touchOutsideText.gameObject.SetActive(false);
                }
            }
            else
            {
                touchOutsideText.gameObject.SetActive(false);
            }
        }
    }
    private void PushStack(UIBasePopup uIBasePopup)
    {
        popupStack.Push(uIBasePopup);
        if (uIBasePopup.IsHideWhenTouchOutside)
        {
            touchOutsideText.gameObject.SetActive(true);
            touchOutsideText.transform.SetAsLastSibling();         
        }
        else
        {
            touchOutsideText.gameObject.SetActive(false);
        }
    }
    public void ShowBlackBg()
    {
        Image blackBgImage = Instantiate(blackBgPrefab, canvasTrans);
        blackBgImage.color = Color.clear;
        blackBgImage.DOFade(0.5f, 0.3f);
        if (blackBgStack == null)
        {
            blackBgStack = new Stack<Image>();
        }
        blackBgStack.Push(blackBgImage);
    }
    public void HideBlackBg()
    {
        Image blackBgImage = blackBgStack?.Pop();
        blackBgImage.DOFade(0.5f, 0.3f).OnComplete(() => Destroy(blackBgImage.gameObject));
    }

    //Show popup prefabs
    public void ShowSetting()
    {
        ShowBlackBg();
        UISetting uiSetting = Instantiate(uiSettingPrefab, canvasTrans);
        uiSetting.Show();
        PushStack(uiSetting);
    }

    public void ShowTutorial1()
    {
        ShowBlackBg();
        UIBasePopup ui = Instantiate(uiTutorial1, canvasTrans);
        ui.Show();
        PushStack(ui);
    }

    public void ShowTutorial2()
    {
        ShowBlackBg();
        UIBasePopup ui = Instantiate(uiTutorial2, canvasTrans);
        ui.Show();
        PushStack(ui);
    }

    public void ShowTutorial3()
    {
        ShowBlackBg();
        UIBasePopup ui = Instantiate(uiTutorial3, canvasTrans);
        ui.Show();
        PushStack(ui);
    }
}
