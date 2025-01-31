using Gameplay.Projectiles;
using UnityEngine;
using VContainer;

namespace Gameplay.Player
{
    public class PlayerGunner : MonoBehaviour
    {
        public bool IsFiring { get; set; }

        [SerializeField] private float m_FireRate       = 8.0f;
        [SerializeField] private int   m_MaxProjectiles = 4;
        [SerializeField] private float m_ProjectileSpeed = 10.0f;
        
        private float                m_Cooldown;
        private int                  m_ProjectilesCount;
        private IProjectileCollision m_Collision;
        private Rigidbody2D          m_Rigidbody2D;
        
        [Inject] private ProjectilesManager m_ProjectilesManager;


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
            
            if (m_Cooldown > 0.0f)
                return;
            
            if (m_ProjectilesCount >= m_MaxProjectiles)
                return;

            // Fire
            m_Cooldown = 1.0f / m_FireRate;
            m_ProjectilesCount++;
            
            Vector2 projectileVelocity = m_Rigidbody2D.linearVelocity + (Vector2)transform.up * m_ProjectileSpeed;
            Projectile projectile = m_ProjectilesManager.Spawn(transform.position, projectileVelocity, m_Collision);
            projectile.OnDespawn += _ => m_ProjectilesCount--;
        }
    }
}