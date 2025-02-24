using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace YandexSdk
{
    public static class YandexGamesEvents
    {
        public static event Action<bool> OnGamePaused = delegate { };
        public static bool IsPaused { get; private set; } = false;

        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            #if !UNITY_EDITOR
            YandexSdkBindEvents(GamePaused, GameResumed);
            #endif
            
            #if UNITY_EDITOR && UNITY_WEBGL
            Application.focusChanged += focused =>
            {
                if (focused)
                    GameResumed();
                else
                    GamePaused();
            };
            #endif
        }
        
        
        [DllImport("__Internal")]
        private static extern void YandexSdkBindEvents(Action gamePaused, Action gameResumed);

        [MonoPInvokeCallback(typeof(Action))]
        private static void GamePaused()
        {
            AudioListener.pause = true;
            
            // Log
            if (YandexGamesSdk.LOGGING)
                Debug.Log("[YandexGamesEvents] Game paused");
            
            // Event
            IsPaused = true;
            OnGamePaused.Invoke(true);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void GameResumed()
        {
            AudioListener.pause = false;
            
            // Log
            if (YandexGamesSdk.LOGGING)
                Debug.Log("[YandexGamesEvents] Game resumed");
            
            // Event
            IsPaused = false;
            OnGamePaused.Invoke(false);
        }
    }
}