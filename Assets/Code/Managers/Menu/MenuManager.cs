using Cysharp.Threading.Tasks;
using UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Managers.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [Inject] private Fade m_Fade;

        public void Play()
        {
            m_Fade.Show(async () =>
            {
                await SceneManager.LoadSceneAsync("Gameplay");
            }).Forget();
        }
    }
}
