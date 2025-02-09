using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;
using VContainer;
using TMPro;

namespace UI.Gameplay
{
    public class ScoreDrawer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_MainText;
        [SerializeField] private TextMeshProUGUI m_AddedScoreText;
        
        [Inject] private readonly ScoreManager m_ScoreManager;
        
        private uint  m_Score;
        private uint  m_AddedScore;
        private uint  m_RealScore;
        private float m_AnimationCooldown;
        
        private UniTask m_ScoreAnimation;

        
        [Inject]
        public void Construct()
        {
            m_Score = m_ScoreManager.Score;
            m_ScoreManager.OnScoreChanged += OnScoreChanged;
            
            m_AddedScoreText.gameObject.SetActive(false);
            UpdateText();
        }
        private void OnDestroy()
        {
            if(m_ScoreManager != null)
                m_ScoreManager.OnScoreChanged -= OnScoreChanged;
        }

        private void OnScoreChanged(uint score)
        {
            uint addedScore = score - m_RealScore;
            m_RealScore = score;
            
            m_AddedScore        += addedScore;
            m_AnimationCooldown =  0.5f;
            UpdateText();
            
            if (m_ScoreAnimation.Status != UniTaskStatus.Pending)
                m_ScoreAnimation = OnScoreAdded();
        }
        private async UniTask OnScoreAdded()
        {
            m_AddedScoreText.gameObject.SetActive(true);
            UpdateText();
            
            while (m_AddedScore > 0)
            {
                if (m_AnimationCooldown > 0.0f)
                {
                    m_AnimationCooldown -= Time.deltaTime;
                    await UniTask.Yield();
                    continue;
                }
                
                m_AddedScore--;
                m_Score++;
                
                UpdateText();

                await UniTask.WaitForSeconds(0.01f);
            }

            m_AddedScoreText.gameObject.SetActive(false);
        }

        private void UpdateText()
        {
            m_AddedScoreText.text = $"+{m_AddedScore}";
            m_MainText.text       = m_Score.ToString("000 000");
        }
    }
}