using System;
using Gameplay.Projectiles;
using Gameplay.UnboundedSpace;
using Other;
using UnityEngine;

namespace Gameplay.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EnemyBehaviour : MonoBehaviour, IUnboundedSpaceTransform, IProjectileCollision
    {
        public Vector2 Position
        {
            get => m_Rigidbody2D.position;
            set => m_Rigidbody2D.position = value;
        }
        public Bounds2D  Bounds => Bounds2D.FromPoint(Position, 1.0f);

        private Rigidbody2D m_Rigidbody2D;
        
        public event Action OnDestroyed = delegate { };
        
        
        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        #region Collision

        public void OnProjectileCollision(ProjectileCollisionData collision) => OnDestroyed.Invoke();
        public void OnCollisionEnter2D(Collision2D other)
        {
            if(!other.collider.TryGetComponent(out IEnemyCollision collisionHandler))
                return;
            
            collisionHandler.OnEnemyCollision(this);
            OnDestroyed.Invoke();
        }

        #endregion

        public void OnSpawn()
        {
            
        }
        public void OnDespawn()
        {
            OnDestroyed            = delegate { };
            m_Rigidbody2D.linearVelocity = Vector2.zero;
        }
    }
}