using Gameplay.Player.Configs;
using Gameplay.Projectiles;
using UnityEngine;
using VContainer;

namespace Gameplay.Player
{
    public class PlayerGunner : MonoBehaviour
    {
        public bool IsFiring { get; set; }

        [SerializeField] private GunConfiguration m_Configuration;
        
        private float                m_Cooldown;
        private int                  m_ProjectilesCount;
        private IProjectileCollision m_Collision;
        private Rigidbody2D          m_Rigidbody2D;
        
        [Inject] private ProjectilesManager m_ProjectilesManager;

        public event System.Action<Projectile> OnFire = delegate { };


        private void Awake()
        {
            m_Collision   = GetComponent<IProjectileCollision>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            m_Cooldown -= Time.fixedDeltaTime;
            
            if (!IsFiring)
                return;
            
            Fire();
        }
        
        private bool CanFire() => m_Cooldown <= 0.0f && m_ProjectilesCount < m_Configuration.MaxProjectiles;
        private void Fire()
        {
            if (!CanFire()) 
                return;
            
            // Reset cooldown
            m_Cooldown = 1.0f / m_Configuration.FireRate;
            m_ProjectilesCount++;
            
            // Spawn projectile
            Vector2 velocity      = m_Rigidbody2D.linearVelocity + (Vector2)transform.up * m_Configuration.ProjectileSpeed;
            float  lifetime       = m_Configuration.ProjectileLifetime;
            
            Projectile projectile = m_ProjectilesManager.Spawn(transform.position, velocity, lifetime, true, ProjectileLayer.Player);
            projectile.OnDespawn += () => m_ProjectilesCount--;
            
            OnFire.Invoke(projectile);
        }
    }
}