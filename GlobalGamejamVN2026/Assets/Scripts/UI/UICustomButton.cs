using DG.Tweening;
using GDC.Managers;
using GDC.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [AddComponentMenu("UI/Custom Button")]
    public class UICustomButton : Button
    {
        public enum State //The order must matches with SelectionState
        {
            NORMAL,
            HOVER,
            PRESS,
            SELECTED,
            DISABLED,
        }

        [SerializeField] private GameObject m_normalTemplate;
        [SerializeField] private GameObject m_hoverTemplate;
        [SerializeField] private GameObject m_pressTemplate;
        [SerializeField] private GameObject m_disabledTemplate;
        [SerializeField] private GameObject m_selectedTemplate;

        [SerializeField] private bool m_isSetStateManual;
        [SerializeField] private Sprite m_backgroundPatternSprite;

        private List<RectTransform> m_backgroundPatterns;
        Sequence m_patternSequence;
        bool m_isPatternAnimating;

        protected override void Awake()
        {
            base.Awake();

            SetupGUI();
            navigation = new Navigation { mode = Navigation.Mode.None };

            SetState(SelectionState.Normal);

            m_isPatternAnimating = false;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            m_backgroundPatterns = GameUtils.FindListChildIgnoreCase<RectTransform>(transform, "BGPattern");
            foreach (var pattern in m_backgroundPatterns)
            {
                Image image = pattern.GetComponent<Image>();
                if (image)
                {
                    if (m_backgroundPatternSprite)
                    {
                        image.enabled = true;
                        image.sprite = m_backgroundPatternSprite;
                    }
                    else
                    {
                        image.enabled = false;
                    }
                }
            }
        }
#endif

        private void SetupGUI()
        {
            if (m_normalTemplate == null) m_normalTemplate = GameUtils.FindChildIgnoreCase(transform, "normal");
            if (m_hoverTemplate == null) m_hoverTemplate = GameUtils.FindChildIgnoreCase(transform, "hover");
            if (m_pressTemplate == null) m_pressTemplate = GameUtils.FindChildIgnoreCase(transform, "press");
            if (m_disabledTemplate == null) m_disabledTemplate = GameUtils.FindChildIgnoreCase(transform, "disabled");
            if (m_selectedTemplate == null) m_selectedTemplate = GameUtils.FindChildIgnoreCase(transform, "selected");

            m_backgroundPatterns = GameUtils.FindListChildIgnoreCase<RectTransform>(transform, "BGPattern");
            foreach (var pattern in m_backgroundPatterns)
            {
                Image image = pattern.GetComponent<Image>();
                if (image)
                {
                    if (m_backgroundPatternSprite)
                    {
                        image.enabled = true;
                        image.sprite = m_backgroundPatternSprite;
                    }
                    else
                    {
                        image.enabled = false;
                    }
                }
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (!m_isSetStateManual)
            {
                SetState(state);
            }
        }

        public void SetStateManual(bool isSetStateManual)
        {
            m_isSetStateManual = isSetStateManual;
        }

        /// <summary>
        /// Suggest call SetStateManual(true) before using this
        /// </summary>
        /// <param name="state"></param>
        public void SetState(State state)
        {
            SetState((SelectionState)state);
        }

        private void SetState(SelectionState state)
        {
            switch(state)
            {
                case SelectionState.Highlighted:
                    SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_BUTTON_HOVER);
                    break;

                case SelectionState.Pressed:
                    SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_BUTTON_CLICK);
                    break;
            }

            bool isSetUnNormalTemplate = false;

            if (m_hoverTemplate)
            {
                isSetUnNormalTemplate |= state == SelectionState.Highlighted;
                m_hoverTemplate.SetActive(state == SelectionState.Highlighted);
            }

            if (m_pressTemplate)
            {
                isSetUnNormalTemplate |= state == SelectionState.Pressed;
                m_pressTemplate.SetActive(state == SelectionState.Pressed);
            }

            if (m_disabledTemplate)
            {
                isSetUnNormalTemplate |= state == SelectionState.Disabled;
                m_disabledTemplate.SetActive(state == SelectionState.Disabled);
            }

            if (m_selectedTemplate)
            {
                isSetUnNormalTemplate |= state == SelectionState.Selected;
                m_selectedTemplate.SetActive(state == SelectionState.Selected);
            }

            if (m_normalTemplate)
            {
                m_normalTemplate.SetActive(state == SelectionState.Normal || !isSetUnNormalTemplate);
            }

            TryDoBackgroundPatternAnimation(state);
        }

        private void TryDoBackgroundPatternAnimation(SelectionState state)
        {
            bool newIsPatternAnimating = state == SelectionState.Highlighted || state == SelectionState.Pressed || state == SelectionState.Selected;
            if (m_isPatternAnimating != newIsPatternAnimating)
            {
                m_patternSequence.Kill();
                m_patternSequence = DOTween.Sequence();
                foreach (var pattern in m_backgroundPatterns)
                {
                    m_isPatternAnimating = newIsPatternAnimating;

                    if (m_isPatternAnimating)
                    {
                        m_patternSequence.Join(pattern.DOAnchorPos(new Vector2(100, 100), 2f).SetEase(Ease.Linear));
                    }
                    else
                    {
                        m_patternSequence.Join(pattern.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutQuad));
                    }
                }

                if (m_isPatternAnimating)
                {
                    m_patternSequence.SetLoops(-1, LoopType.Restart);
                }    
            }
        }
    }
}
