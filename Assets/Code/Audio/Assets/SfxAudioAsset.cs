using Audio.Values;
using UnityEngine;

namespace Audio.Assets
{
    [CreateAssetMenu(fileName = "New audio", menuName = "Game/Audio/VFX Sound", order = 0)]
    public class SfxAudioAsset : WorldAudioAsset
    {
        public override   float      Volume       => m_Volume.Value;
        public override   float      Pitch        => m_Pitch.Value;
        internal override float      SpatialBlend => m_SpatialBlend;
        internal override RangeValue Distance     => m_Distance;
        
        [SerializeField] private FloatValue m_Volume       = new(1.0f);
        [SerializeField] private FloatValue m_Pitch        = new(1.0f);
        [SerializeField] private float      m_SpatialBlend = 0.0f;
        [SerializeField] private RangeValue m_Distance     = new(5.0f, 30.0f);
    }
}