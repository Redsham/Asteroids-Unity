using Audio.Sources;
using Cysharp.Threading.Tasks;
using LitMotion;
using Managers;
using Managers.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using VContainer;

namespace UI.Gameplay.Elements
{
    public class WaveNotify : MonoBehaviour
    {
        #region Fields

        [Header("Components")]
        [SerializeField] private          TextMeshProUGUI m_Text;
        [SerializeField] private          Image           m_Background;
        
        [Header("Other")]
        [SerializeField] private LocalizedString      m_WaveStarted;
        [SerializeField] private InterfaceAudioSource m_AudioSource;

        [Inject]         private readonly WavesManager    m_Manager;

        #endregion


        [Inject]
        public void Construct()
        {
            m_Manager.OnWaveStarted += OnWaveStarted;
            gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            if(m_Manager == null)
                return;
            
            m_Manager.OnWaveStarted -= OnWaveStarted;
        }


        private void OnWaveStarted(uint wave) => Notify(m_WaveStarted).Forget();
        private async UniTaskVoid Notify(LocalizedString localizedText)
        {
            m_Background.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            
            m_Text.text = string.Empty;
            m_Text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            
            Vector2 endPosition   = m_Background.rectTransform.anchoredPosition;
            Vector2 startPosition = endPosition + new Vector2(0.0f, -100.0f);
            
            gameObject.SetActive(true);
            m_AudioSource.Play();

            await LMotion.Create(0.0f, 1.0f, 0.25f)
                         .WithEase(Ease.OutCubic)
                         .Bind(time =>
                         {
                             m_Background.color = new Color(1.0f, 1.0f, 1.0f, time);
                             m_Background.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, time);
                         });


            #region Text Animation

            string text = await localizedText.GetLocalizedStringAsync(m_Manager.Wave);
            bool[] charFlags = new bool[text.Length];
            
            foreach (char _ in text)
            {
                // Randomly select a character
                int index;
                do
                {
                    index = Random.Range(0, text.Length);
                } while (charFlags[index]);
                
                // Set the character flag
                charFlags[index] = true;
                m_Text.text      = string.Empty;
                
                // Display the text
                for (int i = 0; i < text.Length; i++)
                    m_Text.text += charFlags[i] ? text[i] : ' ';
                
                await UniTask.WaitForSeconds(0.01f);
            }
            
            await UniTask.WaitForSeconds(1.0f);

            #endregion
            
            await LMotion.Create(1.0f, 0.0f, 0.25f)
                         .WithEase(Ease.InCubic)
                         .Bind(time =>
                         {
                             m_Background.color = new Color(1.0f, 1.0f, 1.0f, time);
                             m_Text.color = new Color(1.0f, 1.0f, 1.0f, time);
                             m_Background.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, time);
                         });
            
            gameObject.SetActive(false);
            m_Background.rectTransform.anchoredPosition = endPosition;
        }
    }
}