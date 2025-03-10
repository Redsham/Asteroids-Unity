using Common;
using UnityEngine;
using VContainer.Unity;

namespace Gameplay.Player.Inputs
{
    public class MobileInput : Input, IFixedTickable
    {
        private Vector2 m_Direction;

        private readonly PIDController m_RotationPidController = new(2.0f, 0.1f, 0.2f);

        
        public void SetDirection(Vector2 direction) => m_Direction = direction;
        public void SetThrust(bool       isHolding) => LinearThrust = isHolding ? 1.0f : 0.0f;
        public void SetFire(bool         isHolding) => IsFiring = isHolding;
        
        
        public void FixedTick()
        {
            // Calculate rotation with PID controller
            float currentAngle    = Vector2.SignedAngle(Vector2.up, Movement.transform.up);
            float targetAngle     = Vector2.SignedAngle(Vector2.up, m_Direction);
            float errorAngle      = Mathf.DeltaAngle(currentAngle, targetAngle);
            float angularThrottle = -m_RotationPidController.Calculate(0.0f, errorAngle, Time.fixedDeltaTime);
            
            AngularThrust = angularThrottle;
        }
    }
}