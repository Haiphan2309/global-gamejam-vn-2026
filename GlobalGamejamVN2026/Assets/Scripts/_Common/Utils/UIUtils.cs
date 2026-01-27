using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GDC.Utils
{
    public class UIUtils
    {
        public static void LockInput()
        {
            EventSystem e = GameObject.FindObjectOfType<EventSystem>();
            if (e != null)  
            {
                e.enabled = false;
            }
        }
        public static void UnlockInput()
        {
            EventSystem e = GameObject.FindObjectOfType<EventSystem>();
            if (e != null)  
            {
                e.enabled = true;
            }   
        }

        public static GameObject GetUIObjectUnderPointer()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            if (results.Count > 0)
            {
                return results[0].gameObject;
            }

            return null;
        }

        public static GameObject TryGetUIObjectUnderPointerWithTag(string tag)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (var result in results)
            {
                if (result.gameObject.CompareTag(tag))
                {
                    return result.gameObject;
                }
            }

            return null;
        }
    }
}
