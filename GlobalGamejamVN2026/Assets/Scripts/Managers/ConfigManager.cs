using UnityEngine;
using GDC.Configuration;

namespace GDC.Managers
{
    public class ConfigManager : MonoBehaviour
    {
        public static ConfigManager Instance {get; private set;}
        public SceneConfig SceneConfig;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }
}
