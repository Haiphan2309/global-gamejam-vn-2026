using GDC.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIBasePopup
{
    [SerializeField] private Button menuBtn, hideButton;
    [SerializeField] private Slider musicSlider, soundSlider;
    [SerializeField] private int maxVolume = 10;
    public override void Show(bool isPlaySound = true, bool isPlayAnimation = true)
    {
        base.Show(isPlaySound, isPlayAnimation);
        Setup();
    }
    private void Setup()
    {
        menuBtn.onClick.AddListener(OnMenu);
        hideButton.onClick.AddListener(() => Hide());

        musicSlider.onValueChanged.AddListener(delegate { OnChangeMusicVolume(); });
        soundSlider.onValueChanged.AddListener(delegate { OnChangeSoundVolume(); });
        musicSlider.maxValue = maxVolume;
        soundSlider.maxValue = maxVolume;
        musicSlider.value = SoundManager.Instance.GetMusicVolume() * maxVolume;
        soundSlider.value = SoundManager.Instance.GetSFXVolume() * maxVolume;
    }
    public override void Hide(bool isPlaySound = true, bool isPlayAnimation = true)
    {
        base.Hide(isPlaySound, isPlayAnimation);
    }
    public void OnMenu()
    {
        //int curChapterIndex = GameplayManager.Instance.chapterData.id;
        //GameManager.Instance.LoadSceneManually(
        //    GDC.Enums.SceneType.MAINMENU,
        //    GDC.Enums.TransitionType.IN,
        //    SoundType.NONE,
        //    cb: () =>
        //    {
        //        //    //GDC.Managers.GameManager.Instance.SetInitData(levelIndex);
        //        GameManager.Instance.LoadMenuLevel(curChapterIndex);
        //    },
        //    true);
    }

    void OnChangeMusicVolume()
    {
        SoundManager.Instance.SetMusicVolume((float)musicSlider.value/maxVolume);
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_TOUCH);
    }
    void OnChangeSoundVolume()
    {
        SoundManager.Instance.SetSFXVolume((float)soundSlider.value / maxVolume);
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_TOUCH);
    }
}
