using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Player.Inputs
{
    public class DefaultInput : Input, IInitializable, IFixedTickable, IDisposable
    {
        [Inject] private InputActionAsset m_InputActionAsset;
        
        private float m_Thrust;
        private float m_Rotation;
        
        public void Initialize()
        {
            InputAction move = m_InputActionAsset["Player/Move"];
            InputAction fire = m_InputActionAsset["Player/Fire"];
            
            move.performed += OnMove;
            move.canceled  += OnMove;

            fire.performed += OnFire;
            fire.canceled  += OnFire;
        }
        public void FixedTick()
        {
            Movement.Thrust(m_Thrust);
            Movement.Rotate(m_Rotation);
        }
        
        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            
            m_Thrust = value.y;
            m_Rotation = value.x;
        }
        private void OnFire(InputAction.CallbackContext context)
        {
            Gunner.IsFiring = context.ReadValueAsButton();
        }

        public void Dispose()
        {
            InputAction move = m_InputActionAsset["Player/Move"];
            InputAction fire = m_InputActionAsset["Player/Fire"];
            
            move.performed -= OnMove;
            move.canceled  -= OnMove;
            
            fire.performed -= OnFire;
            fire.canceled  -= OnFire;
        }
    }
}