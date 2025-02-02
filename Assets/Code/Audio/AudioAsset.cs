using Audio.Values;
using UnityEngine;

namespace Audio
{
    public abstract class AudioAsset : ScriptableObject
    {
        public AudioClip Clip        => m_Clip;
        public bool      Loop        => m_Loop;
        public bool      PlayOnAwake => m_PlayOnAwake;
        
        [SerializeField] private AudioClip m_Clip;
        [SerializeField] private bool      m_Loop;
        [SerializeField] private bool      m_PlayOnAwake;
        
        public abstract float      Volume       { get; }
        public abstract float      Pitch        { get; }
    }
    
    public abstract class WorldAudioAsset : AudioAsset
    {
        internal abstract float SpatialBlend { get; }
        internal abstract RangeValue Distance     { get; }
    }
}