using UnityEngine;

namespace Gameplay.Cameras
{
    public abstract class CameraShakeBase
    {
        public CameraShakeBase(float duration, float magnitude)
        {
            Duration  = duration;
            Magnitude = magnitude;
        }

        
        public float Duration  { get; }
        public float Magnitude { get; }
        public float Time      { get; private set; }
        public bool  IsDone    => Time >= 1f;


        public Vector2 Update(float deltaTime)
        {
            Time += deltaTime / Duration;
            return GetShake();
        }
        protected abstract Vector2 GetShake();
    }
}