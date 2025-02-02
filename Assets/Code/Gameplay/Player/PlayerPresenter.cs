using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Gameplay.Player
{
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_SpriteRenderer;
        [SerializeField] private ParticleSystem m_ThrusterParticles;

        [Inject] private readonly PlayerBehaviour m_Behaviour;
        [Inject] private readonly PlayerMovement  m_Movement;

        private ParticleSystem.EmissionModule m_ThrusterEmission;
        private float                         m_ThrusterEmissionRate;

        
        [Inject]
        public void Construct()
        {
            m_ThrusterEmission = m_ThrusterParticles.emission;
            m_ThrusterEmissionRate = m_ThrusterEmission.rateOverTime.constant;
            
            m_Behaviour.OnInvulnerabilityChanged += isInvulnerable =>
            {
                if(!isInvulnerable)
                    return;
                
                InvulnerabilityAnimation().Forget();
            };
        }
        private void Update()
        {
            m_ThrusterEmission.rateOverTime = m_Movement.LinearThrust * m_ThrusterEmissionRate;
        }
        
        
        private async UniTaskVoid InvulnerabilityAnimation()
        {
            const float BLINK_INTERVAL = 0.1f;
            
            while (m_Behaviour.Invulnerable)
            {
                m_SpriteRenderer.enabled = !m_SpriteRenderer.enabled;
                await UniTask.WaitForSeconds(BLINK_INTERVAL);
            }
            
            m_SpriteRenderer.enabled = true;
        }
    }
}