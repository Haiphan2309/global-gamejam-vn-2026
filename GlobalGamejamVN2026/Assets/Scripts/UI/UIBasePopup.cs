using DG.Tweening;
using GDC.Managers;
using UnityEngine;

public class UIBasePopup : MonoBehaviour
{
    private RectTransform panelRect;
    [SerializeField] private bool isHideWhenTouchOutside; //Check if popup can be hide when touch outside the popup

    private const float showDuration = 0.5f;
    private const float hideDuration = 0.5f;

    public bool IsHideWhenTouchOutside
    {
        get { return isHideWhenTouchOutside; }
    }

    private void Awake()
    {
        panelRect = GetComponent<RectTransform>();
    }

    public virtual void Show(bool isPlaySound = true, bool isPlayAnimation = true)
    {
        if (isPlaySound)
        {
            SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_UI_SHOW);
        }

        DOTween.Kill(panelRect);

        if (isPlayAnimation)
        {
            CanvasGroup canvasGroup = panelRect.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0;
                canvasGroup.DOFade(1, showDuration);
                canvasGroup.interactable = false;
            }    

            panelRect.localScale = Vector2.zero;
            panelRect.DOScale(1, showDuration).SetEase(Ease.OutBack).OnComplete(() => OnShowAnimationFinished());
        }
        else
        {
            panelRect.localScale = Vector2.one;
        }
    }

    public virtual void Hide(bool isPlaySound = true, bool isPlayAnimation = true)
    {
        PopupManager.Instance.ClosePopup();
        if (isPlaySound)
        {
            SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_UI_HIDE);
        }

        DOTween.Kill(panelRect);

        if (isPlayAnimation)
        {
            CanvasGroup canvasGroup = panelRect.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(0, showDuration);
            }
            panelRect.DOScale(0, hideDuration).SetEase(Ease.InBack).OnComplete(() => OnHideAnimationFinished());
        }
        else
        {
            panelRect.localScale = Vector2.zero;
        }

        Destroy(gameObject, 1);
    }

    public virtual void OnShowAnimationFinished()
    {
        CanvasGroup canvasGroup = panelRect.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
        }
    }

    public virtual void OnHideAnimationFinished()
    {

    }
}
