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
        #region Fields

        public IEnemyTarget    Target          { get; private set; }
        public ProjectileLayer ProjectileLayer => ProjectileLayer.Enemy;
        public abstract uint   Score           { get; }
        
        public Vector2 Position
        {
            get => m_Rigidbody2D.position;
            set => m_Rigidbody2D.position = value;
        }
        public Vector2 Velocity
        {
            get => m_Rigidbody2D.linearVelocity;
            set => m_Rigidbody2D.linearVelocity = value;
        }
        public Bounds2D  Bounds => Bounds2D.FromPoint(Position, 1.0f);
        
        private Rigidbody2D m_Rigidbody2D;

        #endregion

        public event Action OnDestroyed = delegate { };
        
        
        private void Awake() => m_Rigidbody2D = GetComponent<Rigidbody2D>();

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

        public void OnSpawn(IEnemyTarget target) => Target = target;
        public void OnDespawn()
        {
            OnDestroyed            = delegate { };
            Velocity = Vector2.zero;
        }
    }
}