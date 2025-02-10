using System;
using System.Runtime.InteropServices;
using AOT;
using Cysharp.Threading.Tasks;

namespace Yandex
{
    public static class YandexGamesSdk
    {
        public const bool LOGGING = true;
        
        #region Initialization

        public static  bool   IsInitialized => GetYandexSdkIsInitialized();
        private static Action s_OnInitialized = delegate { };
        
        public static async UniTask Initialize()
        {
            YandexSdkInitialize(OnInitializeSuccessCallback);
            
            await UniTask.WaitUntil(() => IsInitialized);
        }
        
        #region External

        [DllImport("__Internal")]
        private static extern bool GetYandexSdkIsInitialized();
        
        [DllImport("__Internal")]
        private static extern void YandexSdkInitialize(Action successCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnInitializeSuccessCallback()
        {
            if (LOGGING)
                UnityEngine.Debug.Log("YandexGamesSdk: Initialized");
            
            s_OnInitialized.Invoke();
        }

        #endregion

        #endregion

        #region Game Layout

        public static void Ready() => YandexSdkGameReady();

        public static bool Playing
        {
            get => s_Playing;
            set
            {
                if (s_Playing == value)
                    return;
                
                s_Playing = value;
                
                if (s_Playing)
                    YandexSdkGameplayStart();
                else
                    YandexSdkGameplayStop();
            }
        }
        private static bool s_Playing = false;
        
        #region External

        [DllImport("__Internal")]
        private static extern void YandexSdkGameReady();
        
        [DllImport("__Internal")]
        private static extern void YandexSdkGameplayStart();
        
        [DllImport("__Internal")]
        private static extern void YandexSdkGameplayStop();

        #endregion

        #endregion
    }
}