using Gameplay.ObjectsPools;
using Gameplay.UnboundedSpace;
using Other;
using UnityEngine;

namespace Gameplay.Projectiles
{
    public class Projectile : MonoBehaviour, IUnboundedSpaceTransform, IPoolGetHandler, IPoolReturnHandler
    {
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        public Vector2  Velocity { get; set; }
        public float    Lifetime { get; set; }
        
        public Bounds2D Bounds   => Bounds2D.FromPoint(Position);
        
        public event System.Action<Projectile> OnDespawn = delegate { };
        
        
        public void FixedTick(float deltaTime)
        {
            Position += Velocity * deltaTime;
            Lifetime -= deltaTime;
            
            transform.up = Velocity.normalized;
        }

        public void OnPoolGet()
        {
            Lifetime = 5.0f; // Reset lifetime
        }
        public void OnPoolReturn()
        {
            OnDespawn.Invoke(this); // Notify that projectile is despawned
            OnDespawn = delegate { }; // Clear all subscribers
        }
    }
}