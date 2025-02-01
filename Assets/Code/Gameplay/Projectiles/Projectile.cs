using System;
using Gameplay.UnboundedSpace;
using Other;
using UnityEngine;

namespace Gameplay.Projectiles
{
    public class Projectile : MonoBehaviour, IUnboundedSpaceTransform
    {
        #region Fields

        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        public Vector2              Velocity        { get; set; }
        
        public float                Lifetime        { get; set; }
        public IProjectileCollision IgnoreCollision { get; set; }
        
        public Bounds2D Bounds  => Bounds2D.FromPoint(Position);
        
        #endregion

        #region Events

        public event Action<IProjectileCollision> OnCollision   = delegate { };
        public event Action                       OnLifetimeEnd = delegate { };
        public event Action                       OnDespawn     = delegate { };
        #endregion
        
        public void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;
            Vector2 newPosition = Position + Velocity * deltaTime;

            RaycastHit2D hit = Physics2D.Linecast(Position, newPosition);
            if(hit && hit.collider.TryGetComponent(out IProjectileCollision collision) && collision != IgnoreCollision)
            {
                Position = hit.point;
                
                collision.OnProjectileCollision(this);
                OnCollision.Invoke(collision);
                
                return;
            }
            
            Position     = newPosition;
            transform.up = Velocity.normalized;

            Lifetime -= deltaTime;
            if(Lifetime <= 0.0f)
                OnLifetimeEnd.Invoke();
        }
        public void Despawn()
        {
            OnDespawn.Invoke();           // Notify that projectile is despawned
            OnDespawn     = delegate { }; // Clear all subscribers
            OnCollision   = delegate { }; // Clear all subscribers
            OnLifetimeEnd = delegate { }; // Clear all subscribers
            
            Velocity      = Vector2.zero; // Reset velocity
            Lifetime      = 0.0f;         // Reset lifetime
        }
    }
}