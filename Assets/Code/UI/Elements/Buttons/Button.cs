using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Elements.Buttons
{
    public class Button : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent OnClick => m_OnClick;
        [SerializeField] private UnityEvent m_OnClick;
        
        public virtual bool Interactable
        {
            get => InternalInteractable;
            set => InternalInteractable = value;
        }
        [SerializeField, InspectorName("Interactable")] 
        protected bool InternalInteractable = true;

        
        private void OnEnable() => Interactable = InternalInteractable;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            if(!Interactable)
                return;
            
            HandleInput();
        }
        protected virtual void HandleInput() => m_OnClick.Invoke();

        private void OnValidate()
        {
            Interactable = InternalInteractable;
        }
    }
}