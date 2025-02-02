using Audio.Assets;
using UnityEngine;

namespace Audio
{
    public interface IAudioManager
    {
        void Play(AudioAsset       asset);
        void PlaySfx(SfxAudioAsset asset, Vector2 position);
    }
}