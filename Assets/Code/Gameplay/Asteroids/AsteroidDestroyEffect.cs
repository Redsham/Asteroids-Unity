using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.Asteroids
{
    [RequireComponent(typeof(ParticleSystem))]
    internal class AsteroidDestroyEffect : MonoBehaviour
    {
        private ParticleSystem                     m_ParticleSystem;
        private IObjectPool<AsteroidDestroyEffect> m_Pool;
        
        
        public void Initialize(IObjectPool<AsteroidDestroyEffect> pool)
        {
            m_ParticleSystem = GetComponent<ParticleSystem>();
            m_Pool           = pool;
        }
        
        public void Play(AsteroidBehaviour asteroid)
        {
            transform.position   = asteroid.Position;
            transform.localScale = Vector3.one * (asteroid.Level + 1.0f);
            m_ParticleSystem.Play();
        }
        private void OnParticleSystemStopped() => m_Pool.Release(this);
    }
}