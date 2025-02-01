using System;
using Gameplay.Player.Inputs;
using UI.Elements.Buttons;
using UI.Elements.Joysticks;
using UI.Joysticks;
using UnityEngine;
using VContainer;
using Input = Gameplay.Player.Inputs.Input;

namespace UI.Gameplay
{
    public class MobileInputUI : MonoBehaviour
    {
        [SerializeField] private Joystick m_DirectionJoystick;

        [SerializeField] private HoldButton m_ThrustButton;
        [SerializeField] private HoldButton m_FireButton;
        
        private MobileInput m_MobileInput;

        
        [Inject]
        public void Construct(Input input)
        {
            if (input is not MobileInput mobileInput)
            {
                gameObject.SetActive(false);
                return;
            }
            
            m_DirectionJoystick.OnValueChanged.AddListener(HandleDirectionJoystick);
            m_ThrustButton.OnHold.AddListener(mobileInput.SetThrust);
            m_FireButton.OnHold.AddListener(mobileInput.SetFire);
            
            m_MobileInput = mobileInput;
        }
        private void OnDestroy()
        {
            m_DirectionJoystick.OnValueChanged.RemoveListener(HandleDirectionJoystick);
            m_ThrustButton.OnHold.RemoveListener(m_MobileInput.SetThrust);
            m_FireButton.OnHold.RemoveListener(m_MobileInput.SetFire);
        }

        
        private void HandleDirectionJoystick(Vector2 value)
        {
            if(!m_DirectionJoystick.IsDragging)
                return;
            
            m_MobileInput.SetDirection(value);
        }
    }
}