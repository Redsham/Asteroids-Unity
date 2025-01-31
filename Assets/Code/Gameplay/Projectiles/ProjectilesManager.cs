using System.Collections.Generic;
using Gameplay.ObjectsPools;
using Gameplay.UnboundedSpace;
using UnityEngine;
using VContainer;

namespace Gameplay.Projectiles
{
    public class ProjectilesManager : MonoBehaviour
    {
        [SerializeField] private GameObjectsPool<Projectile> m_ProjectilesPool;


        private readonly          List<Projectile>        m_ActiveProjectiles = new();
        [Inject] private readonly UnboundedSpaceBehaviour m_UnboundedSpace;
        
        public void Awake() => m_ProjectilesPool.Initialize(transform);
        public void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            for (int i = m_ActiveProjectiles.Count - 1; i >= 0; i--)
            {
                m_ActiveProjectiles[i].FixedTick(deltaTime);
                
                // Despawn projectile if lifetime is over
                if (m_ActiveProjectiles[i].Lifetime <= 0)
                    Despawn(m_ActiveProjectiles[i]);
            }
        }
        
        
        public Projectile Spawn(Vector2 position, Vector2 velocity)
        {
            Projectile projectile = m_ProjectilesPool.Get();
            
            projectile.Velocity = velocity;
            projectile.transform.position = position;
            
            m_UnboundedSpace.Register(projectile);
            m_ActiveProjectiles.Add(projectile);
            
            return projectile;
        }
        public void Despawn(Projectile projectile)
        {
            m_UnboundedSpace.Unregister(projectile);
            
            m_ActiveProjectiles.Remove(projectile);
            m_ProjectilesPool.Return(projectile);
        }
    }
}