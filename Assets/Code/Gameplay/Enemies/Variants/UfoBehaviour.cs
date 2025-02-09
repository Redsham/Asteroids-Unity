using Audio;
using Audio.Assets;
using Gameplay.Projectiles;
using Gameplay.UnboundedSpace;
using UnityEngine;
using VContainer;

namespace Gameplay.Enemies.Variants
{
    public abstract class UfoBehaviour : EnemyBehaviour
    {
        public float MaxSpeed     => m_MaxSpeed;
        public float Acceleration => m_Acceleration;
        
        public float FireRate     => m_FireRate;
        public float ProjectileSpeed    => m_ProjectileSpeed;
        public float ProjectileLifeTime => m_ProjectileLifeTime;

        [Header("UFO Movement")] 
        [SerializeField] private float m_MaxSpeed     = 5.0f;
        [SerializeField] private float m_Acceleration = 1.0f;
        
        [Header("UFO Shooting")]
        [SerializeField] private float         m_FireRate           = 1.0f;
        [SerializeField] private float         m_ProjectileSpeed    = 5.0f;
        [SerializeField] private float         m_ProjectileLifeTime = 5.0f;
        [SerializeField] private SfxAudioAsset m_FireSound;
        
        [Inject] private readonly ProjectilesManager    m_ProjectilesManager;
        [Inject] private readonly UnboundedSpaceManager m_UnboundedSpaceManager;


        protected Vector2 GetPathToTarget() => m_UnboundedSpaceManager.ShortestPath(Position, Target.Position);
        protected Vector2 GetPredictedShotDirection(out float time)
        {
            time = float.MaxValue;
            
            Vector2 toTarget         = GetPathToTarget().normalized;
            Vector2 relativeVelocity = Target.Velocity - Velocity;

            float a = Vector2.Dot(relativeVelocity, relativeVelocity) - m_ProjectileSpeed * m_ProjectileSpeed;
            float b = 2 * Vector2.Dot(toTarget, relativeVelocity);
            float c = Vector2.Dot(toTarget, toTarget);

            if (Mathf.Abs(a) < 1e-6)
                return toTarget.normalized;

            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
                return toTarget.normalized;

            float sqrtD = Mathf.Sqrt(discriminant);
            float t1    = (-b - sqrtD) / (2 * a);
            float t2    = (-b + sqrtD) / (2 * a);

            switch (t1)
            {
                case > 0 when t2 > 0:
                    time = Mathf.Min(t1, t2);
                    break;
                case > 0:
                    time = t1;
                    break;
                default:
                {
                    if (t2 > 0)
                        time = t2;
                    else
                        return toTarget.normalized;
                    break;
                }
            }

            Vector2 intercept = Target.Position + Target.Velocity * time;
            return (intercept - Position).normalized;
        }
        protected Vector2 AddDeviationByRotation(Vector2 direction, float rads)
        {
            float angle = Random.Range(-rads, rads);
            return new Vector2(direction.x * Mathf.Cos(angle) - direction.y * Mathf.Sin(angle),
                               direction.x * Mathf.Sin(angle) + direction.y * Mathf.Cos(angle));
        }
        protected void Fire(Vector2 direction)
        {
            m_ProjectilesManager.Spawn(transform.position, direction * m_ProjectileSpeed,
                                       m_ProjectileLifeTime, true, ProjectileLayer.Enemy);
            
            IUniAudioManager.Active.PlayWorld(m_FireSound, transform.position);
        }
    }
}