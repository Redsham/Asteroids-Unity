using Cysharp.Threading.Tasks;
using LitMotion;
using Managers;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI.Gameplay
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;
        
        [SerializeField] private TextMeshProUGUI m_ScoreText;
        [SerializeField] private TextMeshProUGUI m_WaveText;

        [Inject] private readonly GameManager  m_GameManager;
        [Inject] private readonly ScoreManager m_ScoreManager;
        [Inject] private readonly WavesManager m_WavesManager;
        
        private bool m_IsInteractable;


        public async UniTaskVoid Show()
        {
            await UniTask.WaitForSeconds(1.5f);
            
            m_ScoreText.text = "000 000";
            m_WaveText.text  = "00";
            
            gameObject.SetActive(true);
            
            await LMotion.Create(0.0f, 1.0f, 0.5f)
                         .WithEase(Ease.OutExpo)
                         .Bind(time => m_CanvasGroup.alpha = time);
            
            PlayScoreAnimation(m_ScoreManager.Score);
            PlayWavesAnimation(m_WavesManager.Wave);
            
            m_IsInteractable = true;
        }
        public async UniTask Hide()
        {
            await LMotion.Create(1.0f, 0.0f, 0.25f)
                         .WithEase(Ease.OutExpo)
                         .Bind(time => m_CanvasGroup.alpha = time);
            
            gameObject.SetActive(false);
        }
        
        private void PlayScoreAnimation(uint score)
        {
            LMotion.Create(0.0f, 1.0f, 2.0f)
                   .WithEase(Ease.OutExpo)
                   .Bind(time => m_ScoreText.text = ((uint) (score * time)).ToString("000 000"));
        }
        private void PlayWavesAnimation(uint waves)
        {
            LMotion.Create(0.0f, 1.0f, 2.0f)
                   .WithEase(Ease.OutExpo)
                   .Bind(time => m_WaveText.text = ((uint) (waves * time)).ToString("00"));
        }

        public void QuitToMenu()
        {
            if (!m_IsInteractable)
                return;
            
            m_IsInteractable = false;
        }
        public void Revive()
        {
            if (!m_IsInteractable)
                return;
            
            m_IsInteractable = false;
            m_GameManager.Revive();
        }
    }
}