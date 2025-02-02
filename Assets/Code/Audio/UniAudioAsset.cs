using Audio.Values;
using UnityEngine;

namespace Audio
{
    public abstract class UniAudioAsset : ScriptableObject
    {
        public abstract AudioClip Clip        { get; }
        public abstract float     Volume      { get; }
        public abstract float     Pitch       { get; }
        public          bool      Loop        => m_Loop;
        public          bool      PlayOnAwake => m_PlayOnAwake;
        
        [SerializeField] private bool      m_Loop;
        [SerializeField] private bool      m_PlayOnAwake;
    }
    
    public abstract class WorldAudioAsset : UniAudioAsset
    {
        public abstract float      SpatialBlend { get; }
        public abstract RangeValue Distance     { get; }
    }
}