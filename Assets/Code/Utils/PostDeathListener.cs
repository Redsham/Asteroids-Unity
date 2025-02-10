using Gameplay.Player;
using UnityEngine;
using VContainer;

namespace Utils
{
    [RequireComponent(typeof(AudioListener))]
    public class PostDeathListener : MonoBehaviour
    {
        [Inject] private readonly PlayerBehaviour m_Player;

        
        [Inject]
        private void Construct()
        {
            m_Player.OnExplode += OnDeath;
            m_Player.OnRevived += OnRevived;
        }
        private void OnDestroy()
        {
            m_Player.OnExplode -= OnDeath;
            m_Player.OnRevived -= OnRevived;
        }

        private void OnValidate() => GetComponent<AudioListener>().enabled = false;

        private void OnDeath() => GetComponent<AudioListener>().enabled = true;
        private void OnRevived() => GetComponent<AudioListener>().enabled = false;
    }
}