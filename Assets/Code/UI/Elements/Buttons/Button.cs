using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Elements.Buttons
{
    public class Button : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent OnClick => m_OnClick;
        [SerializeField] private UnityEvent m_OnClick;


        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            HandleInput();
        }
        
        protected virtual void HandleInput() => m_OnClick.Invoke();
    }
}