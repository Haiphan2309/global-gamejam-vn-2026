using UnityEngine;
using System;
using GDC.Enums;
using GDC.Common;
using GDC.Home;

namespace GDC.Events
{
    public class GameEvents
    {
        public static Action<SceneType, System.Action> LOAD_SCENE;
        public static Action<SceneType, System.Action> LOAD_SCENE_ASYNC;
        public static Action<SceneType, System.Action> UNLOAD_SCENE;

        public static Action<bool> ON_LOADING;
    }
    public class GameConstants
    {
        public static float TRANSITION_TIME = 1f;
        public static float LOADING_TIME = 1f;
    }
}
