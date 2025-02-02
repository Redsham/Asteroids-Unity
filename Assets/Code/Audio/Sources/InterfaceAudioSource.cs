using Audio.Assets;
using UnityEngine;

namespace Audio.Sources
{
    public class InterfaceAudioSource : UniAudioSource<InterfaceAudioAsset>
    {
        protected override void Setup(AudioSource audioSource)
        {
            base.Setup(audioSource);
            
            audioSource.spatialBlend = 0.0f;
            audioSource.spatialize   = false;
            
            audioSource.bypassEffects         = true;
            audioSource.bypassListenerEffects = true;
            audioSource.bypassReverbZones     = true;
            
            ApplyVolumeAndPitch();
        }

        public override void Play()
        {
            ApplyVolumeAndPitch();
            AudioSource.Play();
        }
    }
}