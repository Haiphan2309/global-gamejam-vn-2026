using System.Collections.Generic;
using UnityEngine;

namespace GDC.Utils
{
    public static class GameUtils
    {
        public static bool CompareVector3(Vector3 v1, Vector3 v2)
        {
            return (Mathf.RoundToInt(v1.x) == Mathf.RoundToInt(v2.x)
                 && Mathf.RoundToInt(v1.y) == Mathf.RoundToInt(v2.y)
                 && Mathf.RoundToInt(v1.z) == Mathf.RoundToInt(v2.z));
        }

        public static Vector3Int ConvertToVector3Int(Vector3 v)
        {
            return new Vector3Int((int)Mathf.Round(v.x), (int)Mathf.Round(v.y), (int)Mathf.Round(v.z));
        }

        /// <summary>
        /// Return the position snap to grid 
        /// </summary>
        /// <param name="position">Vector3 position</param>
        public static Vector3 SnapToGrid(Vector3 position)
        {
            return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));
        }

        public static GameObject FindChildIgnoreCase(Transform transform, string name)
        {
            return FindChildIgnoreCaseRecursive(transform, name);
        }

        private static GameObject FindChildIgnoreCaseRecursive(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (string.Equals(child.name, name, System.StringComparison.OrdinalIgnoreCase))
                {
                    return child.gameObject;
                }

                GameObject found = FindChildIgnoreCaseRecursive(child, name);
                if (found != null)
                    return found;
            }

            return null;
        }

        public static T FindChildIgnoreCase<T>(Transform transform, string name) where T : Component
        {
            return FindChildIgnoreCaseRecursive<T>(transform, name);
        }

        private static T FindChildIgnoreCaseRecursive<T>(Transform parent, string name) where T : Component
        {
            foreach (Transform child in parent)
            {
                if (string.Equals(child.name, name, System.StringComparison.OrdinalIgnoreCase))
                {
                    T comp = child.GetComponent<T>();
                    if (comp != null)
                        return comp;
                }

                T found = FindChildIgnoreCaseRecursive<T>(child, name);
                if (found != null)
                    return found;
            }

            return null;
        }

        public static List<T> FindListChildIgnoreCase<T>(Transform transform, string name) where T : Component
        {
            List<T> result = new List<T>();
            FindChildRecursive(transform, name, result);
            return result;
        }

        private static void FindChildRecursive<T>(Transform parent, string name, List<T> result) where T : Component
        {
            foreach (Transform child in parent)
            {
                if (string.Equals(child.name, name, System.StringComparison.OrdinalIgnoreCase))
                {
                    T comp = child.GetComponent<T>();
                    if (comp)
                        result.Add(comp);
                }

                FindChildRecursive(child, name, result);
            }
        }
    }
}
