using UnityEngine;

namespace Gameplay.Enemies.Variants
{
    public class BigUfo : UfoBehaviour
    {
        public override int Score => 200;

        [Header("Other")]
        [SerializeField] private float m_DirectionChangeCooldown = 1.0f;
        
        private Vector2 m_RandomDirection;
        private float   m_DirectionChangeTime;
        
        private float m_FireCooldown;

        private void FixedUpdate()
        {
            TickMovement();
            TickFire();
        }
        
        private void TickMovement()
        {
            Velocity = Vector2.Lerp(Velocity, m_RandomDirection * MaxSpeed, Time.fixedDeltaTime);
            
            if(Time.time < m_DirectionChangeTime)
                return;
            
            m_RandomDirection = Random.insideUnitCircle.normalized;
            m_DirectionChangeTime = Time.time + m_DirectionChangeCooldown * Random.Range(0.5f, 1.2f);
        }
        private void TickFire()
        {
            m_FireCooldown -= Time.fixedDeltaTime;
            if(m_FireCooldown > 0.0f)
                return;
            
            m_FireCooldown = 1.0f / FireRate;
            Fire(GetPredictedShotDirection(out float time));
        }
    }
}