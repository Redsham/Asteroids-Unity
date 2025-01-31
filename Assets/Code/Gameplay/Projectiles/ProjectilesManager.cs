using System.Collections.Generic;
using Gameplay.UnboundedSpace;
using UnityEngine;
using Utils.ObjectsPools;
using VContainer;

namespace Gameplay.Projectiles
{
    public class ProjectilesManager : MonoBehaviour
    {
        [SerializeField] private GameObjectsPool<Projectile> m_ProjectilesPool;


        private readonly          List<Projectile>        m_Projectiles = new();
        [Inject] private readonly UnboundedSpaceManager m_UnboundedSpace;
        
        public void Awake() => m_ProjectilesPool.Initialize(transform);
        public void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            for (int i = m_Projectiles.Count - 1; i >= 0; i--)
            {
                Projectile projectile = m_Projectiles[i];
                projectile.FixedTick(deltaTime);
                
                // Skip projectile if it's not in use
                if (!projectile.IsUsing)
                {
                    i--;
                    continue;
                }
                
                // Despawn projectile if lifetime is over
                if (projectile.Lifetime <= 0)
                {
                    Despawn(projectile);
                    i--;
                }
            }
        }
        
        
        public Projectile Spawn(Vector2 position, Vector2 velocity, IProjectileCollision ignoreCollision = null)
        {
            Projectile projectile = m_ProjectilesPool.Get();
            
            projectile.Velocity = velocity;
            projectile.transform.position = position;
            projectile.IgnoreCollision = ignoreCollision;
            projectile.OnCollision += collision => Despawn(projectile);
            
            m_UnboundedSpace.Register(projectile);
            m_Projectiles.Add(projectile);
            
            return projectile;
        }
        public void Despawn(Projectile projectile)
        {
            m_UnboundedSpace.Unregister(projectile);
            
            m_Projectiles.Remove(projectile);
            m_ProjectilesPool.Return(projectile);
        }
    }
}