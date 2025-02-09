using Other;
using UnityEngine;

namespace Gameplay.Enemies.Variants
{
    public class SmallUfo : UfoBehaviour
    {
        public override int Score => 1000;

        [Header("Other")]
        [SerializeField] private float m_StopDistance = 5.0f;
        
        private readonly PIDController m_PID_Horizontal = new();
        private readonly PIDController m_PID_Vertical   = new();
        private          float         m_Cooldown;
        
        
        private void FixedUpdate()
        {
            TickMovement();
            TickFire();
        }
        private void Update()
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, (Velocity / MaxSpeed).x * -35.0f);
        }

        private void TickMovement()
        {
            Vector2 targetDirection = GetPathToTarget();
            Vector2 targetVelocity  = targetDirection.magnitude > m_StopDistance
                ? targetDirection.normalized * MaxSpeed : Vector2.zero;
            
            Vector2 currentVelocity = Velocity;
            Velocity += Vector2.ClampMagnitude(new Vector2(
               m_PID_Horizontal.Calculate(targetVelocity.x, currentVelocity.x, Time.fixedDeltaTime),
               m_PID_Vertical.Calculate(targetVelocity.y, currentVelocity.y, Time.fixedDeltaTime)
           ), Acceleration) * Time.fixedDeltaTime;
            
            Velocity = Vector2.ClampMagnitude(Velocity, MaxSpeed);
        }
        private void TickFire()
        {
            m_Cooldown -= Time.fixedDeltaTime;
            if(m_Cooldown > 0.0f)
                return;
            
            Vector2 direction = GetPredictedShotDirection(out float time);
            if(time > ProjectileLifeTime)
                return;

            m_Cooldown = 1.0f / FireRate;
            Fire(AddDeviationByRotation(direction, 10.0f * Mathf.Deg2Rad));
        }
    }
}