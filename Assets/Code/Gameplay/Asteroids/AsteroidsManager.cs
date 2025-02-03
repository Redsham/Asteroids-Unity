using System;
using System.Collections.Generic;
using Gameplay.UnboundedSpace;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Gameplay.Asteroids
{
    public class AsteroidsManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private AsteroidBehaviour     m_Prefab;
        [SerializeField] private AsteroidDestroyEffect m_DestroyEffect;
        
        private IObjectPool<AsteroidBehaviour>     m_AsteroidsPool;
        private IObjectPool<AsteroidDestroyEffect> m_DestroyEffectPool;
        
        private readonly          List<AsteroidBehaviour> m_Asteroids = new();
        
        [Inject] private readonly UnboundedSpaceManager m_UnboundedSpace;
        [Inject] private readonly IObjectResolver       m_ObjectResolver;

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
            instance => Destroy(instance.gameObject));
            
            m_DestroyEffectPool = new ObjectPool<AsteroidDestroyEffect>(
                () => {
                    AsteroidDestroyEffect instance = m_ObjectResolver.Instantiate(m_DestroyEffect, transform);
                    instance.Initialize(m_DestroyEffectPool);
                    return instance; 
                },
            instance => instance.gameObject.SetActive(true),
            instance => instance.gameObject.SetActive(false),
            instance => Destroy(instance.gameObject));
        }
        private void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 position = Random.insideUnitCircle * 10.0f;
                Vector2 velocity = Random.insideUnitCircle.normalized * Random.Range(3.0f, 5.0f);
                Spawn(position, velocity, 2);
            }
        }
        private void OnDestroy() => m_AsteroidsPool.Clear();

        public void Spawn(Vector2 position, Vector2 velocity, int level)
        {
            // Get asteroid from pool
            AsteroidBehaviour asteroid = m_AsteroidsPool.Get();
            
            // Initialize asteroid
            asteroid.Initialize(level);
            
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
                int childLevel = asteroid.Level - 1;
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
            asteroid.Despawn();
            
            m_UnboundedSpace.Unregister(asteroid);
            m_Asteroids.Remove(asteroid);
            
            m_AsteroidsPool.Release(asteroid);
        }
    }
}