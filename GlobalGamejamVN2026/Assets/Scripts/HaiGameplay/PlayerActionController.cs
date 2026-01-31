using GDC.Enums;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public class PlayerActionController : MonoBehaviour
    {
        public InteractableObject m_currentHoverObject;
        public InteractableObject m_currentInteractedObject;

        private void Start()
        {
            Setup();
        }

        public void Setup()
        {
            m_currentInteractedObject = null;
        }

        private void Update()
        {
            Vector3 pos = Input.mousePosition;
            pos.z = 10f;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(pos);
            Collider2D[] cols = Physics2D.OverlapPointAll(mousePos);

            if (cols != null && cols.Length > 0)
            {
                CheckHoverObject(cols);

                if (Input.GetMouseButtonDown(0))
                {
                    CheckInteractWithObject(cols, mousePos);
                }
            }

            CheckExitObject(cols);

            if (Input.GetMouseButton(0))
            {
                CheckDragObject(mousePos);
            }

            if (Input.GetMouseButtonUp(0))
            {
                CheckDropObject(mousePos);
            }

            if (Input.GetMouseButtonDown(0))
            {
            }
        }

        private void CheckInteractWithObject(Collider2D[] cols, Vector2 interactPos)
        {
            InteractableObject best = null;
            int bestPriority = int.MinValue;

            foreach (var col in cols)
            {
                if (col.CompareTag("Nose"))
                {
                    FaceController.Instance.PressOnNose();
                }

                var interactable = col.GetComponentInParent<InteractableObject>();
                if (interactable == null || !interactable.IsAllowInteract())
                {
                    continue;
                }

                if (interactable.InteractPriority > bestPriority)
                {
                    bestPriority = interactable.InteractPriority;
                    best = interactable;
                }
            }

            if (best != m_currentInteractedObject)
            {
                m_currentInteractedObject = best;
                m_currentInteractedObject?.OnReceiveActionFromPlayer(interactPos);
            }
        }

        public void OnObjectEscapeFromDrag()
        {
            m_currentInteractedObject = null;
        }

        private void CheckHoverObject(Collider2D[] cols)
        {
            InteractableObject best = null;
            int bestPriority = int.MinValue;

            foreach (var col in cols)
            {
                var interactable = col.GetComponentInParent<InteractableObject>();
                if (interactable == null || !interactable.IsAllowInteract() || interactable == m_currentInteractedObject)
                {
                    continue;
                }

                if (interactable.InteractPriority > bestPriority)
                {
                    bestPriority = interactable.InteractPriority;
                    best = interactable;
                }
            }

            if (best != m_currentHoverObject)
            {
                if (m_currentHoverObject != m_currentInteractedObject)
                {
                    m_currentHoverObject?.OnExit();
                }
                m_currentHoverObject = best;
                m_currentHoverObject?.OnHover();
            }
        }

        private void CheckExitObject(Collider2D[] cols)
        {
            if (m_currentHoverObject == null)
            {
                return;
            }

            bool isObjectStillOnHover = 
                cols != null && 
                cols.Length > 0 && 
                cols.Any(col => col.GetComponentInParent<InteractableObject>() && col.GetComponentInParent<InteractableObject>().gameObject == m_currentHoverObject.gameObject);

            if (!isObjectStillOnHover)
            {
                m_currentHoverObject.OnExit();
                m_currentHoverObject = null;
            }
        }

        private void CheckDragObject(Vector2 dragPos)
        {
            m_currentInteractedObject?.OnDrag(dragPos);
        }

        private void CheckDropObject(Vector2 dropPos)
        {
            m_currentInteractedObject?.OnDrop(dropPos); //It should immediately enter hover state again
            m_currentInteractedObject = null;
        }
    }
}
