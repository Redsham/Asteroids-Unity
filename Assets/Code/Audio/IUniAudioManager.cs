using UnityEngine;

namespace Audio
{
    public interface IUniAudioManager
    {
        internal static IUniAudioManager Active;
        
        void Play(UniAudioAsset        asset);
        void PlayWorld(WorldAudioAsset asset, Vector2 position);
    }
}