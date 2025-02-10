using Gameplay.Player;
using LitMotion;
using UnityEngine;
using VContainer;

namespace UI.Gameplay
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameScreensDrawer : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_GameUI;
        
        [Inject] private readonly PlayerBehaviour m_Player;
        
        private MotionHandle m_MotionHandle;


        [Inject]
        public void Construct()
        {
            m_Player.OnDeath += OnDeath;
            m_Player.OnRevived += OnRevived;
        }
        private void OnDestroy()
        {
            m_Player.OnDeath -= OnDeath;
            m_Player.OnRevived -= OnRevived;
        }
        
        private void OnDeath()
        {
            m_MotionHandle.TryComplete();
            
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.25f)
                                 .WithOnComplete(() =>
                                 {
                                     m_GameUI.alpha = 1.0f;
                                     gameObject.SetActive(false);
                                 })
                                 .WithEase(Ease.OutBounce)
                                 .Bind(time => m_GameUI.alpha = 1.0f - time);
        }
        private void OnRevived()
        {
            m_MotionHandle.TryComplete();
            
            m_GameUI.alpha = 0.0f;
            gameObject.SetActive(true);
            
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.25f)
                                 .WithEase(Ease.OutBounce)
                                 .Bind(time => m_GameUI.alpha = time);
        }
    }
}