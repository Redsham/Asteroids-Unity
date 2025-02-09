using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Player.Inputs
{
    public class DesktopInput : Input, IInitializable, IDisposable
    {
        [Inject] private readonly InputActionAsset m_InputActionAsset;
        
        public void Initialize()
        {
            InputAction move = m_InputActionAsset["Player/Move"];
            InputAction fire = m_InputActionAsset["Player/Fire"];
            
            move.performed += OnMove;
            move.canceled  += OnMove;

            fire.performed += OnFire;
            fire.canceled  += OnFire;
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
        
        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            
            LinearThrust  = value.y;
            AngularThrust = -value.x;
        }
        private void OnFire(InputAction.CallbackContext context) => IsFiring = context.ReadValueAsButton();
    }
}