using Audio;
using Audio.Assets;
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

        [SerializeField] protected InterfaceAudioAsset ClickSound;

        
        private void OnEnable() => Interactable = InternalInteractable;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            if(!Interactable)
                return;
            
            HandleInput();
        }
        protected virtual void HandleInput()
        {
            PlaySound(ClickSound);
            m_OnClick.Invoke();
        }
        
        protected void PlaySound(InterfaceAudioAsset sound)
        {
            if (Interactable && sound != null)
                IUniAudioManager.Active.PlayInterface(sound);
        }

        private void OnValidate()
        {
            Interactable = InternalInteractable;
        }
    }
}