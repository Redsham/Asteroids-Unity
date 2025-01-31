using Other;
using UnityEngine;
using VContainer.Unity;

namespace Gameplay.Player.Inputs
{
    public class MobileInput : Input, IFixedTickable
    {
        private Vector2 m_Direction;
        private bool m_IsThrusting;

        private readonly PIDController m_RotationPidController = new(2.0f, 0.1f, 0.2f);

        
        public void SetDirection(Vector2 direction) => m_Direction = direction;
        public void SetThrust(bool isHolding) => m_IsThrusting = isHolding;
        public void SetFire(bool isHolding) => Gunner.IsFiring = isHolding;

        
        public void FixedTick()
        {
            // Calculate rotation with PID controller
            float currentAngle = Vector2.SignedAngle(Vector2.up, Movement.transform.up);
            float targetAngle  = Vector2.SignedAngle(Vector2.up, Vector2.ClampMagnitude(m_Direction, 1.0f));
            float errorAngle   = Mathf.DeltaAngle(currentAngle, targetAngle);
            float angularThrottle = m_RotationPidController.Calculate(0.0f, errorAngle, Time.fixedDeltaTime);
            
            Movement.Rotate(angularThrottle);
            Movement.Thrust(m_IsThrusting ? 1.0f : 0.0f);
        }
    }
}