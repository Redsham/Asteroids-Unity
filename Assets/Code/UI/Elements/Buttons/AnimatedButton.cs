using LitMotion;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Elements.Buttons
{
    public class AnimatedButton : Button, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        public override bool Interactable
        {
            get => InternalInteractable;
            set
            {
                InternalInteractable = value;
                
                m_HoverFactor = 0.0f;
                m_ClickFactor = 1.0f;
                
                foreach (Graphic graphic in GetComponentsInChildren<Graphic>())
                    graphic.color = value ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }
        }
        
        public bool IsHovered
        {
            get => m_IsHovered;
            private set
            {
                if (m_IsHovered == value)
                    return;
                
                // Animate the color of the button
                m_HoverMotion.TryComplete();
                m_HoverMotion = LMotion.Create(value ? 0.0f : 1.0f, value ? 1.0f : 0.0f, value ? 0.25f : 0.5f)
                                       .WithEase(Ease.OutExpo)
                                       .Bind(time => m_HoverFactor = time);
                
                m_IsHovered = value;
            }
        }
        private bool m_IsHovered;

        private float m_HoverFactor;
        private float m_ClickFactor = 1.0f;
        
        private MotionHandle m_HoverMotion;
        private MotionHandle m_ClickMotion;

        #endregion


        protected override void HandleInput()
        {
            base.HandleInput();
            
            m_ClickMotion.TryComplete();
            m_ClickMotion = LMotion.Punch.Create(1.0f, 0.1f, 0.15f)
                                   .WithFrequency(2)
                                   .WithDampingRatio(2.0f)
                                   .WithEase(Ease.OutExpo)
                                   .Bind(time => m_ClickFactor = time);
        }

        public void OnPointerEnter(PointerEventData eventData) => IsHovered = true;
        public void OnPointerExit(PointerEventData eventData) => IsHovered = false;

        private void Update()
        {
            transform.localScale = Vector3.one * (m_ClickFactor + m_HoverFactor * (Interactable ? 1.0f : 0.0f) * 0.05f);
        }
    }
}