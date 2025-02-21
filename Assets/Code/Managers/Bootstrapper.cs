using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YandexSdk;

namespace Managers
{
    public class Bootstrapper : MonoBehaviour
    {
        private async UniTaskVoid Start()
        {
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