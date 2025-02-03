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

        protected override void PlaySelf()
        {
            ApplyVolumeAndPitch();
            AudioSource.Play();
        }
        protected override void PlayOnManager() => IUniAudioManager.Active.PlayWorld(Asset, transform.position);
    }
}