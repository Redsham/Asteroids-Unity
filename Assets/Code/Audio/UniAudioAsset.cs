using Audio.Values;
using UnityEngine;

namespace Audio
{
    public abstract class UniAudioAsset : ScriptableObject
    {
        public abstract AudioClip Clip          { get; }
        public abstract float     Volume        { get; }
        public abstract float     Pitch         { get; }
        public          bool      Loop          => m_Loop;
        public          bool      PlayOnAwake   => m_PlayOnAwake;
        public          bool      PlayOnManager => m_PlayOnManager;
        
        [SerializeField] private bool m_Loop          = false;
        [SerializeField] private bool m_PlayOnAwake   = false;
        [SerializeField] private bool m_PlayOnManager = true;
    }
    
    public abstract class WorldAudioAsset : UniAudioAsset
    {
        public abstract float      SpatialBlend { get; }
        public abstract RangeValue Distance     { get; }
    }
}