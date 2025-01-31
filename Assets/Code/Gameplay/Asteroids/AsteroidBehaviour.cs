using System.Linq;
using Gameplay.Projectiles;
using Gameplay.UnboundedSpace;
using Other;
using UnityEngine;
using Utils.ObjectsPools;

namespace Gameplay.Asteroids
{
    [RequireComponent(typeof(LineRenderer), typeof(PolygonCollider2D), typeof(Rigidbody2D))]
    public class AsteroidBehaviour : MonoBehaviour, IPoollable, IUnboundedSpaceTransform, IProjectileCollision, IPoolInitializer, IPoolReturnHandler
    {
        public int  Level   { get; set; }
        public bool IsUsing { get; set; }
        
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
        
        public Bounds2D Bounds  => Bounds2D.FromPoint(Position, 2.0f);
        
        private Rigidbody2D       m_Rigidbody;
        private LineRenderer      m_LineRenderer;
        private PolygonCollider2D m_PolygonCollider;

        public event System.Action<AsteroidBehaviour> OnDestroy = delegate { };
        
        
        public void OnProjectileCollision(Projectile projectile) => OnDestroy.Invoke(this);
        private void OnCollisionEnter2D(Collision2D other)
        {
            if(!other.collider.TryGetComponent(out IAsteroidCollisionHandler collisionHandler))
                return;
            
            collisionHandler.OnAsteroidCollision(this);
            OnDestroy.Invoke(this);
        }

        public void OnPoolInitialize()
        {
            m_LineRenderer    = GetComponent<LineRenderer>();
            m_PolygonCollider = GetComponent<PolygonCollider2D>();
            m_Rigidbody       = GetComponent<Rigidbody2D>();
        }
        public void Generate(int level)
        {
            Vector2[] vertices = AsteroidsGenerator.GenerateAsteroidVertices(1.0f + level);
            
            m_LineRenderer.positionCount = vertices.Length;
            m_LineRenderer.SetPositions(vertices.Select(x => (Vector3)x).ToArray());
            
            m_PolygonCollider.pathCount = 1;
            m_PolygonCollider.SetPath(0, vertices);
        }
        public void OnPoolReturn() => OnDestroy = delegate { };
    }
}