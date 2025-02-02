using UnityEngine;

namespace Audio.Sources
{
    public class WorldAudioSource : UniAudioSource<WorldAudioAsset>
    {
        protected override void Setup(AudioSource audioSource)
        {
            base.Setup(audioSource);

            audioSource.spatialize   = true;
            audioSource.spatialBlend = Asset.SpatialBlend;
            audioSource.minDistance  = Asset.Distance.Min;
            audioSource.maxDistance  = Asset.Distance.Max;
            
            ApplyVolumeAndPitch();
        }

        public override void Play()
        {
            ApplyVolumeAndPitch();
            AudioSource.Play();
        }
    }
}