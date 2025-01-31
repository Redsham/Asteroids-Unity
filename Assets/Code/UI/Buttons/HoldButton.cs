using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Code.UI.Buttons
{
    public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsHolding
        {
            get => m_IsHolding;
            set
            {
                m_IsHolding = value;
                m_OnHold.Invoke(value);
            }
        }
        private bool m_IsHolding;
        
        public UnityEvent<bool> OnHold => m_OnHold;
        [SerializeField] private UnityEvent<bool> m_OnHold = new();


        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            HandleInput(true);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            HandleInput(false);
        }
        
        protected virtual void HandleInput(bool isHolding) => IsHolding = isHolding;
    }
}