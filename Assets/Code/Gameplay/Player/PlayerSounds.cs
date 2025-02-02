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
        [SerializeField] private SfxAudioAsset m_Shot;
        
        [Inject] private PlayerMovement m_Movement;
        [Inject] private PlayerGunner   m_Gunner;
        
        [Inject] private IUniAudioManager  m_AudioManager;
        
        
        [Inject]
        public void Construct()
        {
            m_Gunner.OnFire += OnFire;
        }
        private void OnDestroy()
        {
            m_Gunner.OnFire -= OnFire;
        }

        private void Update()
        {
            m_Thruster.Pitch = m_Thruster.Volume = Mathf.Lerp(m_Thruster.Volume, m_Movement.LinearThrust, Time.deltaTime * 5.0f);
        }

        private void OnFire(Projectile projectile) => m_AudioManager.PlaySfx(m_Shot, m_Gunner.transform.position);
    }
}