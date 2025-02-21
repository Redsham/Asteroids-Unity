using Audio.Assets;
using UnityEngine;

namespace Audio.Music
{
    [CreateAssetMenu(menuName = "Game/Audio/Music/PlaylistAsset")]
    public class PlaylistAsset : ScriptableObject
    {
        public InterfaceAudioAsset[] AudioAssets => m_AudioAssets;
        [SerializeField] private InterfaceAudioAsset[] m_AudioAssets;
        
        private int m_LastIndex = -1;
        public InterfaceAudioAsset GetNext()
        {
            if (m_AudioAssets.Length == 0)
                return null;
            
            if (m_AudioAssets.Length == 1)
                return m_AudioAssets[0];
            
            int index = Random.Range(0, m_AudioAssets.Length);
            while (index == m_LastIndex)
                index = Random.Range(0, m_AudioAssets.Length);
            
            m_LastIndex = index;
            return m_AudioAssets[index];
        }
    }
}