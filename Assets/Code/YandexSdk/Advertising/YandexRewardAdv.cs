using System;
using System.Runtime.InteropServices;
using AOT;
using Cysharp.Threading.Tasks;

namespace YandexSdk.Advertising
{
    public static class YandexRewardAdv
    {
        #region Fields

        public static YandexAdsStatus Status   { get; private set; } = YandexAdsStatus.Closed;
        public static bool            Rewarded { get; private set; } = false;

        #endregion

        #region Events

        private static Action         s_OpenCallback;
        private static Action         s_RewardCallback;
        private static Action         s_CloseCallback;
        private static Action<string> s_ErrorCallback;

        #endregion


        public static async UniTask<AdsResult> Show()
        {
            Status   = YandexAdsStatus.Loading;
            Rewarded = false;
            
            #if UNITY_EDITOR
            return await ShowEditor();
            #endif
            
            YandexSdkShowRewardedVideo(OnOpenCallback, OnRewardedCallback, OnCloseCallback, OnErrorCallback);
            
            // Wait for the ad is loaded
            await UniTask.WaitWhile(() => Status == YandexAdsStatus.Loading);
            
            // If the ad failed to load, return the status
            if (Status == YandexAdsStatus.Failed)
                return AdsResult.Failed;
            
            // Wait for the ad to close
            await UniTask.WaitUntil(() => Status == YandexAdsStatus.Closed);
            
            // Return the status
            return new AdsResult(Rewarded, Status);
        }
        
        #region Editor
        #if UNITY_EDITOR
        
        private static async UniTask<AdsResult> ShowEditor()
        {
            await UniTask.WaitForSeconds(0.5f);
            Status = YandexAdsStatus.Showing;
            await UniTask.WaitForSeconds(0.5f);
            return new AdsResult(true, YandexAdsStatus.Closed);
        }
        
        #endif
        #endregion
        
        #region External

        [DllImport("__Internal")]
        private static extern void YandexSdkShowRewardedVideo(Action openCallback, Action rewardedCallback, Action closeCallback, Action<string> errorCallback);

        #region Callbacks
        
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOpenCallback() => Status = YandexAdsStatus.Showing;

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnRewardedCallback() => Rewarded = true;

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnCloseCallback() =>Status = YandexAdsStatus.Closed;

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnErrorCallback(string errorMessage) => Status = YandexAdsStatus.Failed;

        #endregion

        #endregion

        
        public readonly struct AdsResult
        {
            public AdsResult(bool rewarded, YandexAdsStatus status)
            {
                Rewarded = rewarded;
                Status   = status;
            }
            
            public readonly bool            Rewarded;
            public readonly YandexAdsStatus Status;
            
            public static AdsResult Failed => new(false, YandexAdsStatus.Failed);
        }
    }
}