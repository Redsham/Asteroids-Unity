using System;
using System.Runtime.InteropServices;
using AOT;
using Cysharp.Threading.Tasks;

namespace Yandex.Advertising
{
    public static class YandexFullscreenAdv
    {
        public static YandexAdsStatus Status { get; private set; } = YandexAdsStatus.Closed;
        
        #region Events

        private static Action         s_OpenCallback;
        private static Action         s_CloseCallback;
        private static Action<string> s_ErrorCallback;

        #endregion


        public static async UniTask<YandexAdsStatus> Show()
        {
            Status = YandexAdsStatus.Loading;
            
            YandexSdkShowFullscreenAdv(OnOpenCallback, OnCloseCallback, OnErrorCallback);
            
            // Wait for the ad is loaded
            await UniTask.WaitWhile(() => Status == YandexAdsStatus.Loading);
            
            // If the ad failed to load, return the status
            if (Status == YandexAdsStatus.Failed)
                return YandexAdsStatus.Failed;
            
            // Wait for the ad to close
            await UniTask.WaitUntil(() => Status == YandexAdsStatus.Closed);
            
            // Return the status
            return YandexAdsStatus.Closed;
        }

        #region External

        [DllImport("__Internal")]
        private static extern void YandexSdkShowFullscreenAdv(Action openCallback, Action closeCallback, Action<string> errorCallback);
        
        #region Callbacks
        
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOpenCallback() => Status = YandexAdsStatus.Showing;

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnCloseCallback() => Status = YandexAdsStatus.Closed;

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnErrorCallback(string error) => Status = YandexAdsStatus.Failed;
        
        #endregion

        #endregion
    }
}