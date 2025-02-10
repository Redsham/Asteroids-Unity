using Cysharp.Threading.Tasks;
using LitMotion;
using Managers;
using Managers.Gameplay;
using TMPro;
using UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using Yandex;
using Yandex.Advertising;

namespace UI.Gameplay
{
    public class GameOverUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private CanvasGroup m_CanvasGroup;
        
        [SerializeField] private TextMeshProUGUI m_ScoreText;
        [SerializeField] private TextMeshProUGUI m_WaveText;

        [SerializeField] private CanvasGroup[] m_Elements;

        [Inject] private readonly GameManager  m_GameManager;
        [Inject] private readonly ScoreManager m_ScoreManager;
        [Inject] private readonly WavesManager m_WavesManager;
        
        [Inject] private readonly Fade m_Fade;
        
        private bool m_IsInteractable;
        
        private MotionHandle m_ScoreAnimation;
        private MotionHandle m_WavesAnimation;

        #endregion


        public async UniTaskVoid Show()
        {
            await UniTask.WaitForSeconds(1.5f);
            
            m_ScoreText.text = "000 000";
            m_WaveText.text  = "00";
            
            gameObject.SetActive(true);
            
            // Hide elements
            foreach (CanvasGroup element in m_Elements)
                element.alpha = 0.0f;
            
            // Show canvas group
            await LMotion.Create(0.0f, 1.0f, 0.5f)
                         .WithEase(Ease.OutSine)
                         .Bind(time => m_CanvasGroup.alpha = time);

            // Show elements
            for (int i = 0; i < m_Elements.Length; i++)
            {
                CanvasGroup element = m_Elements[i];
                LMotion.Create(0.0f, 1.0f, 0.5f)
                       .WithDelay(0.1f * i)
                       .WithEase(Ease.OutExpo)
                       .Bind(time => element.alpha = time);
            }
            await UniTask.WaitForSeconds(0.1f * m_Elements.Length + 0.5f);

            // Play animations
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
            
            m_ScoreAnimation.TryComplete();
            m_WavesAnimation.TryComplete();
        }
        
        private void PlayScoreAnimation(uint score)
        {
            m_ScoreAnimation = LMotion.Create(0.0f, 1.0f, 2.0f)
                                   .WithEase(Ease.OutExpo)
                                   .Bind(time => m_ScoreText.text = ((uint) (score * time)).ToString("000 000"));
        }
        private void PlayWavesAnimation(uint waves)
        {
            m_WavesAnimation = LMotion.Create(0.0f, 1.0f, 2.0f)
                   .WithEase(Ease.OutExpo)
                   .Bind(time => m_WaveText.text = ((uint) (waves * time)).ToString("00"));
        }

        public void QuitToMenu()
        {
            if (!m_IsInteractable)
                return;

            m_Fade.Show(async () =>
            {
                Debug.Log("Quit to menu");
                await SceneManager.LoadSceneAsync("Menu");
            }).Forget();
            m_IsInteractable = false;
        }
        public void Revive()
        {
            if (!m_IsInteractable)
                return;

            ReviveRoutine().Forget();
            m_IsInteractable = false;
        }
        
        private async UniTaskVoid ReviveRoutine()
        {
            YandexRewardAdv.AdsResult result = await YandexRewardAdv.Show();
            
            if (result.Status == YandexAdsStatus.Failed)
            {
                m_IsInteractable = true;
                return;
            }
            
            if (result.Rewarded)
                m_GameManager.Revive();
            else
                m_IsInteractable = true;
        }
    }
}