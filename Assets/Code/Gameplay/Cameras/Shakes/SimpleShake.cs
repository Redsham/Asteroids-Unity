using UnityEngine;

namespace Gameplay.Cameras.Shakes
{
    public class SimpleShake : CameraShakeBase
    {
        public SimpleShake(float duration, float magnitude) : base(duration, magnitude) { }
        
        protected override Vector2 GetShake()
        {
            float   angle     = Random.Range(0f, Mathf.PI * 2f);
            Vector2 direction = new(Mathf.Cos(angle), Mathf.Sin(angle));
            
            return direction * Magnitude * (1f - Time);
        }
    }
}