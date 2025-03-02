using UI.Elements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class ReviveView : ModalWindow
    {
        public UnityEvent OnRevive => m_OnRevive;

        [Header("Revive View")]
        [SerializeField] private Image m_ReviveTimerImage;
        [SerializeField] private float m_ReviveTime = 3.0f;

        [Header("Events")]
        [SerializeField] private UnityEvent m_OnRevive;

        private float Timer
        {
            get => m_ReviveTimer;
            set
            {
                m_ReviveTimer = value;
                m_ReviveTimerImage.fillAmount = m_ReviveTimer / m_ReviveTime;
            }
        }
        private float m_ReviveTimer = 0.0f;


        public override void Show()
        {
            Timer = m_ReviveTime;
            base.Show();
        }

        public void Revive()
        {
            m_OnRevive.Invoke();
            Hide();
        }
        
        private void Update()
        {
            if(!IsVisible)
                return;
            
            Timer -= Time.deltaTime;
            if (Timer <= 0.0f)
                Hide();
        }
    }
}