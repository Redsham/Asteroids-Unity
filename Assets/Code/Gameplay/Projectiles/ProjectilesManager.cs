using Gameplay.UnboundedSpace;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Gameplay.Projectiles
{
    public class ProjectilesManager : MonoBehaviour
    {
        [Inject] private readonly UnboundedSpaceManager m_UnboundedSpace;
        [SerializeField] private Projectile             m_Prefab;

        private IObjectPool<Projectile> m_ProjectilesPool;

        private void Awake()
        {
            m_ProjectilesPool = new ObjectPool<Projectile>(
            () => Instantiate(m_Prefab, transform),
            instance => {
                instance.gameObject.SetActive(true);
            },
            instance => instance.gameObject.SetActive(false),
            x => Destroy(x.gameObject));
        }
        
        public Projectile Spawn(Vector2 position, Vector2 velocity, float lifetime = 2.0f, IProjectileCollision ignoreCollision = null)
        {
            Projectile projectile = m_ProjectilesPool.Get();

            projectile.Lifetime        = lifetime;
            projectile.Velocity        = velocity;
            projectile.Position        = position;
            projectile.IgnoreCollision = ignoreCollision;
            
            projectile.OnCollision        += _ => Despawn(projectile);
            projectile.OnLifetimeEnd      += () => Despawn(projectile);
            
            m_UnboundedSpace.Register(projectile);
            
            return projectile;
        }
        public void Despawn(Projectile projectile)
        {
            projectile.Despawn();
            m_UnboundedSpace.Unregister(projectile);
            m_ProjectilesPool.Release(projectile);
        }
    }
}