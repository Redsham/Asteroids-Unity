using System.Threading;
using Cysharp.Threading.Tasks;
using Other;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;
using YandexSdk;

namespace Managers
{
    public class Bootstrapper : IAsyncStartable
    {
        [Inject] private readonly Preferences m_Preferences;


        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            await LocalizationSettings.InitializationOperation;
            await UniTask.WaitUntil(() => m_Preferences.IsInitialized);
            await YandexSdkInit();
            await SceneManager.LoadSceneAsync("Menu");
        }

        private static async UniTask YandexSdkInit()
        {
            await YandexGamesSdk.Initialize();
            YandexGamesSdk.Ready();
        }
    }
}