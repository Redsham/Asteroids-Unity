using System;
using System.Collections.Generic;
using Gameplay.UnboundedSpace;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Gameplay.Enemies
{
    public class EnemiesManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private EnemyBehaviour[] m_Prefabs;
        
        private readonly Dictionary<Type, IObjectPool<EnemyBehaviour>> m_Pools   = new();
        private readonly List<EnemyBehaviour>                          m_Enemies = new();
        
        [Inject] private readonly UnboundedSpaceManager m_UnboundedSpace;
        [Inject] private readonly IObjectResolver       m_ObjectResolver;

        #endregion

        #region Events

        public event Action<EnemyBehaviour> OnEnemyDestroyed = delegate { };
        public event Action                 OnEnemiesCleared = delegate { };

        #endregion

        
        private void OnDestroy() => m_Enemies.Clear();

        public EnemyBehaviour GetFromPool(Type type)
        {
            if (!m_Pools.TryGetValue(type, out IObjectPool<EnemyBehaviour> pool))
            {
                // Create new pool
                pool = new ObjectPool<EnemyBehaviour>(
                    () =>
                    {
                        int index = Array.FindIndex(m_Prefabs, prefab => prefab.GetType() == type);
                        
                        EnemyBehaviour instance = Instantiate(m_Prefabs[index]);
                        m_ObjectResolver.Inject(instance);
                        instance.gameObject.SetActive(false);
                        
                        return instance;
                    },
                    instance => instance.gameObject.SetActive(true),
                    instance => instance.gameObject.SetActive(false),
                    instance => Destroy(instance.gameObject));
            
                m_Pools.Add(type, pool);
            }
            
            // Get instance from pool
            return pool.Get();
        }
        public void ReleaseToPool(EnemyBehaviour enemy)
        {
            if (!m_Pools.TryGetValue(enemy.GetType(), out IObjectPool<EnemyBehaviour> pool))
                return;
            
            pool.Release(enemy);
        }
        
        public void Spawn(Type type, Vector2 position, IEnemyTarget target)
        {
            EnemyBehaviour enemy = GetFromPool(type);

            enemy.OnSpawn(target);
            
            enemy.transform.position = position;
            enemy.OnDestroyed += () =>
            {
                // Invoke events
                OnEnemyDestroyed.Invoke(enemy);
                if (m_Enemies.Count == 0)
                    OnEnemiesCleared.Invoke();
                
                // Despawn enemy
                Despawn(enemy);
            };
            
            // Register enemy
            m_UnboundedSpace.Register(enemy);
            m_Enemies.Add(enemy);
        }
        public void Despawn(EnemyBehaviour enemy)
        {
            enemy.OnDespawn();
            
            m_UnboundedSpace.Unregister(enemy);
            m_Enemies.Remove(enemy);
            
            ReleaseToPool(enemy);
        }
    }
}