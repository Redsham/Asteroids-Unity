using Audio.Sources;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.Enemies
{
    [RequireComponent(typeof(ParticleSystem))]
    internal class EnemyDestroyEffect : MonoBehaviour
    {
        private WorldAudioSource              m_AudioSource;
        private ParticleSystem                m_ParticleSystem;
        private IObjectPool<EnemyDestroyEffect> m_Pool;
        
        
        public void Initialize(IObjectPool<EnemyDestroyEffect> pool)
        {
            m_AudioSource    = GetComponent<WorldAudioSource>();
            m_ParticleSystem = GetComponent<ParticleSystem>();
            m_Pool           = pool;
        }
        
        public void Play(EnemyBehaviour enemy)
        {
            // Play particle system
            transform.position   = enemy.Position;
            m_ParticleSystem.Play();
            
            // Play sound
            m_AudioSource.Play();
        }
        private void OnParticleSystemStopped() => m_Pool.Release(this);
    }
}