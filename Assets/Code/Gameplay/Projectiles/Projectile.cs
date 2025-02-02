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

        public event Action<ProjectileCollisionData> OnCollision   = delegate { };
        public event Action                       OnLifetimeEnd = delegate { };
        public event Action                       OnDespawn     = delegate { };
        #endregion
        
        public void FixedUpdate()
        {
            PhysicsUpdate();
            LifetimeUpdate();
        }
        
        private void PhysicsUpdate()
        {
            // Calculate new position
            float deltaTime = Time.fixedDeltaTime;
            Vector2 newPosition = Position + Velocity * deltaTime;
            
            // Check collision
            if (CheckCollision(ref newPosition)) return;
            
            // Update position and rotation
            Position     = newPosition;
            transform.up = Velocity.normalized;
        }
        private void LifetimeUpdate()
        {
            Lifetime -= Time.deltaTime;
            
            if(Lifetime <= 0.0f)
                OnLifetimeEnd.Invoke();
        }
        
        private bool CheckCollision(ref Vector2 newPosition)
        {
            RaycastHit2D hit = Physics2D.Linecast(Position, newPosition);
            
            // Check if hit object has IProjectileCollision component and it's not the same as IgnoreCollision
            if (!hit || !hit.collider.TryGetComponent(out IProjectileCollision collision) || collision == IgnoreCollision)
                return false;
            
            // Set new position to hit point
            newPosition = hit.point;
            
            // Notify collision
            ProjectileCollisionData collisionData = new(this, collision, hit);
            
            collision.OnProjectileCollision(collisionData);
            if (!collisionData.Ignored)
                OnCollision.Invoke(collisionData);
            
            return true;
        }
        
        public void Despawn()
        {
            OnDespawn.Invoke();           // Notify that projectile is despawned
            OnDespawn     = delegate { }; // Clear all subscribers
            OnCollision   = delegate { }; // Clear all subscribers
            OnLifetimeEnd = delegate { }; // Clear all subscribers
            
            Velocity = Vector2.zero; // Reset velocity
            Lifetime = 0.0f;         // Reset lifetime
        }
    }
}