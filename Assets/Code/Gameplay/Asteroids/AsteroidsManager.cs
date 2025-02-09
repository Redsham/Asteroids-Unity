using System;
using System.Collections.Generic;
using Gameplay.UnboundedSpace;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Gameplay.Asteroids
{
    public class AsteroidsManager : MonoBehaviour
    {
        public const int MAX_ASTEROIDS = 10;
        
        #region Fields
        
        public IReadOnlyList<AsteroidBehaviour> Asteroids => m_Asteroids;
        public int Count => m_Asteroids.Count;

        [SerializeField] private AsteroidBehaviour     m_Prefab;
        [SerializeField] private AsteroidDestroyEffect m_DestroyEffect;
        
        private IObjectPool<AsteroidBehaviour>     m_AsteroidsPool;
        private IObjectPool<AsteroidDestroyEffect> m_DestroyEffectPool;
        
        private readonly          List<AsteroidBehaviour> m_Asteroids = new(MAX_ASTEROIDS);
        
        [Inject] private readonly UnboundedSpaceManager m_UnboundedSpace;

        #endregion

        #region Events

        public event Action<AsteroidBehaviour> OnAsteroidDestroyed = delegate { };
        public event Action                    OnAsteroidsCleared  = delegate { };

        #endregion


        private void Awake()
        {
            m_AsteroidsPool = new ObjectPool<AsteroidBehaviour>(
            () => Instantiate(m_Prefab, transform),
            instance => instance.gameObject.SetActive(true),
            instance => instance.gameObject.SetActive(false),
            instance => Destroy(instance.gameObject), maxSize: MAX_ASTEROIDS);
            
            m_DestroyEffectPool = new ObjectPool<AsteroidDestroyEffect>(
                () => {
                    AsteroidDestroyEffect instance = Instantiate(m_DestroyEffect, transform);
                    instance.Initialize(m_DestroyEffectPool);
                    return instance; 
                },
            instance => instance.gameObject.SetActive(true),
            instance => instance.gameObject.SetActive(false),
            instance => Destroy(instance.gameObject));
        }
        private void OnDestroy() => m_AsteroidsPool.Clear();

        public void Spawn(Vector2 position, Vector2 velocity, AsteroidLevel level)
        {
            // Get asteroid from pool
            AsteroidBehaviour asteroid = m_AsteroidsPool.Get();
            
            // Initialize asteroid
            asteroid.OnSpawn(level);
            
            // Initialize asteroid
            asteroid.Velocity = velocity;
            asteroid.Position = position;
            asteroid.OnDestroy += () =>
            {
                // Invoke events
                OnAsteroidDestroyed.Invoke(asteroid);
                if (m_Asteroids.Count == 0)
                    OnAsteroidsCleared.Invoke();
                
                // Play destroy effect
                AsteroidDestroyEffect effect = m_DestroyEffectPool.Get();
                effect.Play(asteroid);
                
                // Calculate child level
                AsteroidLevel childLevel = asteroid.Level - 1;
                Vector2 childVelocity = Vector2.Perpendicular(asteroid.Velocity);
                Vector2 parentVelocity = asteroid.Velocity;
                
                // Despawn asteroid
                Despawn(asteroid);

                // Spawn smaller asteroids
                if (asteroid.Level <= 0) return;
                
                Spawn(asteroid.Position, childVelocity + parentVelocity, childLevel);            
                Spawn(asteroid.Position, -childVelocity + parentVelocity, childLevel);            
            };
            
            // Register asteroid
            m_UnboundedSpace.Register(asteroid);
            m_Asteroids.Add(asteroid);
        }
        public void Despawn(AsteroidBehaviour asteroid)
        {
            asteroid.OnDespawn();
            
            m_UnboundedSpace.Unregister(asteroid);
            m_Asteroids.Remove(asteroid);
            
            m_AsteroidsPool.Release(asteroid);
        }

        public void Clear()
        {
            foreach (AsteroidBehaviour asteroid in m_Asteroids)
            {
                asteroid.OnDespawn();
                m_UnboundedSpace.Unregister(asteroid);
                m_AsteroidsPool.Release(asteroid);
            }
            
            m_Asteroids.Clear();
        }
    }
}