using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerBehaviour))]
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_SpriteRenderer;
        [SerializeField] private ParticleSystem m_ThrusterParticles;

        private PlayerBehaviour m_Behaviour;
        private PlayerMovement  m_Movement;

        private ParticleSystem.EmissionModule m_ThrusterEmission;
        
        private float m_ThrusterEmissionRate;

        
        private void Awake()
        {
            m_Behaviour = GetComponent<PlayerBehaviour>();
            m_Movement  = GetComponent<PlayerMovement>();
            
            m_ThrusterEmission = m_ThrusterParticles.emission;
            m_ThrusterEmissionRate = m_ThrusterEmission.rateOverTime.constant;
        }
        private void Start()
        {
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