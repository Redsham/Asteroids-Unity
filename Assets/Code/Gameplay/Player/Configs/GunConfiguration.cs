using System;
using UnityEngine;

namespace Gameplay.Player.Configs
{
    [Serializable]
    public struct GunConfiguration
    {
        public float FireRate           => m_FireRate;
        public int   MaxProjectiles     => m_MaxProjectiles;
        
        public float ProjectileSpeed    => m_ProjectileSpeed;
        public float ProjectileLifetime => m_ProjectileLifetime;
        
        
        [SerializeField] private float m_FireRate;
        [SerializeField] private int   m_MaxProjectiles;
        
        [SerializeField] private float m_ProjectileSpeed;
        [SerializeField] private float m_ProjectileLifetime;
    }
}