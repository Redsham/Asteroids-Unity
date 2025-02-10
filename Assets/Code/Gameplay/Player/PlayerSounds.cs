using Audio;
using Audio.Assets;
using Audio.Sources;
using Gameplay.Projectiles;
using UnityEngine;
using VContainer;

namespace Gameplay.Player
{
    public class PlayerSounds : MonoBehaviour
    {
        [SerializeField] private WorldAudioSource m_Thruster;
        [SerializeField] private SfxAudioAsset    m_Shot;
        [SerializeField] private SfxAudioAsset    m_Damage;
        
        [Inject] private readonly PlayerMovement  m_Movement;
        [Inject] private readonly PlayerGunner    m_Gunner;
        [Inject] private readonly PlayerBehaviour m_Player;


        [Inject]
        public void Construct()
        {
            m_Gunner.OnFire += OnFire;
            m_Player.OnDamaged += OnDamage;
        }

        private void OnDestroy()
        {
            m_Gunner.OnFire -= OnFire;
            m_Player.OnDamaged -= OnDamage;
        }

        private void Update()
        {
            m_Thruster.Pitch = m_Thruster.Volume = Mathf.Lerp(m_Thruster.Volume, m_Movement.LinearThrust, Time.deltaTime * 5.0f);
        }

        private void OnFire(Projectile projectile) => IUniAudioManager.Active.PlayWorld(m_Shot, m_Gunner.transform.position);
        private void OnDamage() => IUniAudioManager.Active.PlayWorld(m_Damage, m_Player.transform.position);
    }
}