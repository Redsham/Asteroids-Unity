using Audio.Values;
using UnityEngine;

namespace Audio.Assets
{
    [CreateAssetMenu(fileName = "New audio", menuName = "Game/Audio/UI Sound", order = 0)]
    public class InterfaceAudioAsset : AudioAsset
    {
        public override float Volume => m_Volume.Value;
        public override float Pitch  => m_Pitch.Value;
        
        [SerializeField] private FloatValue m_Volume = new(1.0f);
        [SerializeField] private FloatValue m_Pitch  = new(1.0f);
    }
}