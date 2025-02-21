using System;
using System.Runtime.InteropServices;
using AOT;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YandexSdk
{
    public static class YandexGamesSdk
    {
        public const bool LOGGING = true;
        
        #region Initialization

        public static  bool IsInitialized => GetYandexSdkIsInitialized();
        private static bool s_IsInitializing = false;
        
        public static async UniTask Initialize()
        {
            s_IsInitializing = true;
            
            #if !UNITY_EDITOR
            YandexSdkInitialize(OnInitializeSuccessCallback);
            await UniTask.WaitWhile(() => s_IsInitializing);
            #else
            await UniTask.WaitForSeconds(1.0f);
            s_IsInitializing = false;
            #endif
            
            if (LOGGING)
                Debug.Log("[YandexGamesSdk] Yandex Games SDK initialized");
        }
        
        #region External

        [DllImport("__Internal")]
        private static extern bool GetYandexSdkIsInitialized();
        
        [DllImport("__Internal")]
        private static extern void YandexSdkInitialize(Action successCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnInitializeSuccessCallback() => s_IsInitializing = false;

        #endregion

        #endregion

        #region Game Layout

        public static void Ready()
        {
            // Send the ready state to the native code
            #if !UNITY_EDITOR
            
            YandexSdkGameReady();
            
            #endif
            
            if (LOGGING)
                Debug.Log("[YandexGamesSdk] Game is marked as ready");
        }

        public static bool Playing
        {
            get => s_Playing;
            set
            {
                if (s_Playing == value)
                    return;
                
                s_Playing = value;
                
                // Send the playing state to the native code
                #if !UNITY_EDITOR
                
                if (s_Playing)
                    YandexSdkGameplayStart();
                else
                    YandexSdkGameplayStop();
                
                #endif
                
                if (LOGGING)
                    Debug.Log($"[YandexGamesSdk] Playing state changed to {s_Playing}");
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