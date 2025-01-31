using Utils.ObjectsPools;
using Gameplay.UnboundedSpace;
using Other;
using UnityEngine;

namespace Gameplay.Projectiles
{
    public class Projectile : MonoBehaviour, IPoollable, IUnboundedSpaceTransform, IPoolGetHandler, IPoolReturnHandler
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
        public bool     IsUsing { get; set; }

        #endregion

        #region Events

        public event System.Action<IProjectileCollision> OnCollision = delegate { }; 
        public event System.Action<Projectile>           OnDespawn   = delegate { };

        #endregion
        
        public void FixedTick(float deltaTime)
        {
            Vector2 newPosition = Position + Velocity * deltaTime;

            RaycastHit2D hit = Physics2D.Linecast(Position, newPosition);
            if(hit && hit.collider.TryGetComponent(out IProjectileCollision collision) && collision != IgnoreCollision)
            {
                Position = hit.point;
                
                collision.OnProjectileCollision(this);
                OnCollision.Invoke(collision);
                
                return;
            }
            
            Position = newPosition;
            Lifetime -= deltaTime;
            
            transform.up = Velocity.normalized;
        }

        #region Pool

        public void OnPoolGet()
        {
            Lifetime = 3.0f; // Reset lifetime
        }
        public void OnPoolReturn()
        {
            OnDespawn.Invoke(this);     // Notify that projectile is despawned
            OnDespawn   = delegate { }; // Clear all subscribers
            OnCollision = delegate { }; // Clear all subscribers
        }

        #endregion
    }
}