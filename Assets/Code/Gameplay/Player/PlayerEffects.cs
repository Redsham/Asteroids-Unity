using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem m_ThrusterParticles;

        private PlayerMovement       m_Movement;
        private ParticleSystem.EmissionModule m_ThrusterEmission;
        
        private float m_ThrusterEmissionRate;

        private void Awake()
        {
            m_Movement = GetComponent<PlayerMovement>();
            
            m_ThrusterEmission = m_ThrusterParticles.emission;
            m_ThrusterEmissionRate = m_ThrusterEmission.rateOverTime.constant;
        }
        private void Update()
        {
            m_ThrusterEmission.rateOverTime = m_Movement.LinearThrust * m_ThrusterEmissionRate;
        }
    }
}