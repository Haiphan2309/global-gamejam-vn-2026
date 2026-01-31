using UnityEngine;

namespace Gameplay
{
    public class InteractableObject : MonoBehaviour
    {
        private Outline outline;

        [Header("Interactable Object Field")]
        [SerializeField] private int m_interactPriority;

        public int InteractPriority => m_interactPriority;

        private bool m_isBeingDragged;
        public bool IsBeingDragged => m_isBeingDragged;

        virtual protected void Setup()
        {
            outline = GetComponent<Outline>();
            if (outline)
            {
                outline.Setup();
                HideOutline();
            }
        }

        virtual public void OnReceiveActionFromPlayer(Vector2 touchPos)
        {
            m_isBeingDragged = true;
        }

        virtual public void OnHover()
        {
            if (outline)
            {
                outline.SetThickness(10);
            }
        }

        virtual public void OnExit()
        {
            HideOutline();
        }

        virtual public void OnDrag(Vector2 dragPos)
        {
        }

        virtual public void OnDrop(Vector2 dropPos)
        {
            m_isBeingDragged = false;
        }

        virtual public bool IsAllowInteract()
        {
            return false;
        }

        private void HideOutline()
        {
            if (outline)
            {
                outline.SetThickness(0);
            }
        }
    }
}
