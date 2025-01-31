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
        
        private float m_Cooldown;
        private int m_ProjectilesCount;
        
        [Inject] private ProjectilesManager m_ProjectilesManager;

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
            
            Projectile projectile = m_ProjectilesManager.Spawn(transform.position, transform.up * 10.0f);
            projectile.OnDespawn += _ => m_ProjectilesCount--;
        }
    }
}