using System;
using System.Collections.Generic;
using Gameplay.UnboundedSpace;
using UnityEngine;
using Utils.ObjectsPools;
using VContainer;
using Random = UnityEngine.Random;

namespace Gameplay.Asteroids
{
    public class AsteroidsManager : MonoBehaviour
    {
        [SerializeField] private  GameObjectsPool<AsteroidBehaviour> m_AsteroidsPool;

        private readonly          List<AsteroidBehaviour> m_Asteroids = new();
        [Inject] private readonly UnboundedSpaceManager   m_UnboundedSpace;
        
        
        public event Action<AsteroidBehaviour> OnAsteroidDestroyed = delegate { };
        public event Action                    OnAsteroidsCleared  = delegate { };
        
        
        public void Awake() => m_AsteroidsPool.Initialize(transform);
        
        public void Spawn(Vector2 position, Vector2 velocity, int level)
        {
            // Get asteroid from pool
            AsteroidBehaviour asteroid = m_AsteroidsPool.Get();
            
            // Initialize asteroid
            asteroid.Level = level;
            asteroid.Velocity = velocity;
            asteroid.Position = position;
            asteroid.OnDestroy += instance =>
            {
                // Invoke events
                OnAsteroidDestroyed.Invoke(instance);
                if (m_Asteroids.Count == 0)
                    OnAsteroidsCleared.Invoke();
                
                // Despawn asteroid
                Despawn(instance);

                // Spawn smaller asteroids
                if (instance.Level <= 0) return;
                for (int i = 0; i < 2; i++)
                {
                    Vector2 newVelocity = Random.insideUnitCircle.normalized * Random.Range(1.0f, 3.0f);
                    Spawn(instance.Position, newVelocity, instance.Level - 1);
                }
            };
            
            // Generate asteroid
            asteroid.Generate(level);
            
            // Register asteroid
            m_UnboundedSpace.Register(asteroid);
            m_Asteroids.Add(asteroid);
        }
        public void Despawn(AsteroidBehaviour asteroid)
        {
            m_UnboundedSpace.Unregister(asteroid);
            m_Asteroids.Remove(asteroid);
            m_AsteroidsPool.Return(asteroid);
        }
    }
}