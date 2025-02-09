using Cysharp.Threading.Tasks;
using LitMotion;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Gameplay
{
    public class WaveNotify : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private          TextMeshProUGUI m_Text;
        [SerializeField] private          Image           m_Background;
        
        [Header("Localization")]

        [Inject]         private readonly WavesManager    m_Manager;
        
        private MotionHandle m_MotionHandle;
        

        [Inject]
        public void Construct()
        {
            m_Manager.OnWaveStarted += OnWaveStarted;
            m_Manager.OnWaveEnded += OnWaveEnded;
            gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            if(m_Manager != null)
                m_Manager.OnWaveStarted -= OnWaveStarted;
        }


        private void OnWaveStarted(int wave) => Notify().Forget();
        private void OnWaveEnded()           => Notify().Forget();
        
        private async UniTaskVoid Notify()
        {

        }
    }
}