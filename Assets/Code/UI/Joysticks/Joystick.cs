using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace UI.Joysticks
{
    public abstract class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public bool IsActive
        {
            get => m_IsActive;
            set
            {
                SetActive(value);
                m_IsActive = value;
            }
        }
        [SerializeField] private bool m_IsActive = true;
        
        public bool IsDragging { get; private set; }

        public Vector2 Value
        {
            get => m_Value;
            set
            {
                m_Value = value;
                m_OnValueChanged.Invoke(value);
            }
        }
        public Vector2 Direction => Value.normalized;
        private Vector2 m_Value;
        
        public                   UnityEvent<Vector2> OnValueChanged => m_OnValueChanged;
        [SerializeField] private UnityEvent<Vector2> m_OnValueChanged = new();
        
        public                   UnityEvent OnBeginDragEvent => m_OnBeginDragEvent;
        [SerializeField] private UnityEvent m_OnBeginDragEvent = new();

        public                    UnityEvent OnEndDragEvent   => m_OnEndDragEvent;
        [SerializeField]  private UnityEvent m_OnEndDragEvent   = new();

        /// <summary>
        /// The screen coordinates of pointer (mouse or touch)
        /// </summary>
        protected Vector2 Point { get; private set; }
        
        

        private void Awake()
        {
            // Register the point action to the Point property
            InputSystemUIInputModule inputSystemUiInputModule = FindFirstObjectByType<InputSystemUIInputModule>();
            inputSystemUiInputModule.point.action.performed += context => Point = context.ReadValue<Vector2>();
            
            Initialize();
            SetActive(IsActive);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsActive) return;
            if (IsDragging) return;
            
            IsDragging = true;

            OnBeginDrag();
            OnBeginDragEvent.Invoke();
            
            HandleInput();
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!IsDragging) return;
            
            HandleInput();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            IsDragging = false;

            OnEndDrag();
            OnEndDragEvent.Invoke();
            
            Value = Vector2.zero;
        }

        public abstract void Initialize();
        
        public abstract void OnBeginDrag();
        public abstract void HandleInput();
        public abstract void OnEndDrag();

        public virtual void SetActive(bool isActive)
        {
            if (isActive) return;
            if (!IsDragging) return;
            IsDragging = false;
                
            OnEndDrag();
            OnEndDragEvent.Invoke();
            
            Value = Vector2.zero;
        }
    }
}