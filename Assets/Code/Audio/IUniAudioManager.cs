using System;
using UnityEngine;

namespace Audio
{
    public interface IUniAudioManager
    {
        public static IUniAudioManager Active { get; private set; }
        internal static void SetActive(IUniAudioManager manager)
        {
            if (Active != null && manager != null)
                throw new InvalidOperationException("UniAudioManager is already initialized.");
            
            Active = manager;
        }
        
        
        void Play(UniAudioAsset        asset);
        void PlayWorld(WorldAudioAsset asset, Vector2 position);
    }
}