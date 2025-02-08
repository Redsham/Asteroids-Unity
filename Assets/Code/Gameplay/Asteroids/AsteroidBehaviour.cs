using System;
using System.Linq;
using Gameplay.Projectiles;
using Gameplay.UnboundedSpace;
using Other;
using UnityEngine;

namespace Gameplay.Asteroids
{
    [RequireComponent(typeof(LineRenderer), typeof(PolygonCollider2D), typeof(Rigidbody2D))]
    public class AsteroidBehaviour : MonoBehaviour, IUnboundedSpaceTransform, IProjectileCollision
    {
        public int  Level   { get; private set; }
        
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        public Vector2 Velocity
        {
            get => m_Rigidbody.linearVelocity;
            set => m_Rigidbody.linearVelocity = value;
        }
        
        public Bounds2D Bounds  => Bounds2D.FromBounds(m_PolygonCollider.bounds);
        
        private Rigidbody2D       m_Rigidbody;
        private LineRenderer      m_LineRenderer;
        private PolygonCollider2D m_PolygonCollider;

        public event Action OnDestroy = delegate { };
        

        private void Awake()
        {
            m_LineRenderer    = GetComponent<LineRenderer>();
            m_PolygonCollider = GetComponent<PolygonCollider2D>();
            m_Rigidbody       = GetComponent<Rigidbody2D>();
        }
        
        #region Collision

        public void OnProjectileCollision(ProjectileCollisionData projectile) => OnDestroy.Invoke();
        private void OnCollisionEnter2D(Collision2D other)
        {
            if(!other.collider.TryGetComponent(out IAsteroidCollision collisionHandler))
                return;
            
            collisionHandler.OnAsteroidCollision(this);
            OnDestroy.Invoke();
        }

        #endregion
        
        public void OnSpawn(int level)
        {
            if(level < 0)
                Debug.LogError("Asteroid level cannot be less than 0");
            
            // Generate asteroid vertices
            Vector2[] vertices = AsteroidsGenerator.GenerateAsteroidVertices(1.0f + level);
            
            // Set line renderer positions
            m_LineRenderer.positionCount = vertices.Length;
            m_LineRenderer.SetPositions(vertices.Select(x => (Vector3)x).ToArray());
            
            // Set polygon collider path
            m_PolygonCollider.pathCount = 1;
            m_PolygonCollider.SetPath(0, vertices);
            
            Level = level;
        }
        public void OnDespawn()
        {
            OnDestroy                   = delegate { }; // Clear all subscribers
            m_Rigidbody.linearVelocity  = Vector2.zero; // Reset velocity
            m_Rigidbody.angularVelocity = 0.0f;         // Reset angular velocity
        }
    }
}