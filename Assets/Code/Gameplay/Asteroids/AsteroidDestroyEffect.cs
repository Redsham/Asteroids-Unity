using Audio;
using Audio.Sources;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Gameplay.Asteroids
{
    [RequireComponent(typeof(ParticleSystem))]
    internal class AsteroidDestroyEffect : MonoBehaviour
    {
        private WorldAudioSource                   m_AudioSource;
        private ParticleSystem                     m_ParticleSystem;
        private IObjectPool<AsteroidDestroyEffect> m_Pool;
        
        
        public void Initialize(IObjectPool<AsteroidDestroyEffect> pool)
        {
            m_AudioSource    = GetComponent<WorldAudioSource>();
            m_ParticleSystem = GetComponent<ParticleSystem>();
            m_Pool           = pool;
        }
        
        public void Play(AsteroidBehaviour asteroid)
        {
            // Play particle system
            transform.position   = asteroid.Position;
            transform.localScale = Vector3.one * (asteroid.Level + 1.0f);
            m_ParticleSystem.Play();
            
            // Play sound
            m_AudioSource.Play();
        }
        private void OnParticleSystemStopped() => m_Pool.Release(this);
    }
}