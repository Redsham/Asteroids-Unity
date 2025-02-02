using Audio.Assets;
using UnityEngine;

namespace Audio
{
    public interface IUniAudioManager
    {
        void Play(UniAudioAsset       asset);
        void PlaySfx(SfxAudioAsset asset, Vector2 position);
    }
}