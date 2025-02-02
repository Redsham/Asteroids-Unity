using Gameplay.Player.Configs;
using Gameplay.UnboundedSpace;
using Other;
using UnityEngine;
using VContainer;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour, IUnboundedSpaceTransform
    {
        public Rigidbody2D           Rigidbody2D    { get; private set; }
        public SpriteRenderer        SpriteRenderer { get; private set; }
        public MovementConfiguration Configuration  => m_Configuration;

        public float LinearThrust
        {
            get => m_LinearThrust;
            set => m_LinearThrust = Mathf.Clamp(value, 0.0f, 1.0f);
        }
        public float AngularThrust
        {
            get => m_AngularThrust;
            set => m_AngularThrust = Mathf.Clamp(value, -1.0f, 1.0f);
        }
        
        public Vector2 Position
        {
            get => Rigidbody2D.position;
            set => Rigidbody2D.MovePosition(value);
        }
        public Bounds2D Bounds => Bounds2D.FromSprite(SpriteRenderer);
        
        [SerializeField] private MovementConfiguration m_Configuration;
        
        [Inject] private UnboundedSpaceManager m_UnboundedSpace;
        private float m_LinearThrust;
        private float m_AngularThrust;

        
        [Inject]
        public void Construct()
        {
            Rigidbody2D    = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            
            m_UnboundedSpace.Register(this);
        }
        private void OnDestroy() => m_UnboundedSpace.Unregister(this);

        private void FixedUpdate()
        {
            // Drag
            Rigidbody2D.linearVelocity = Vector2.Lerp(Rigidbody2D.linearVelocity, Vector2.zero, m_Configuration.LinearDrag * Time.fixedDeltaTime);
            Rigidbody2D.angularVelocity = Mathf.Lerp(Rigidbody2D.angularVelocity, 0.0f, m_Configuration.AngularDrag * Time.fixedDeltaTime);
            
            // Thrust
            Rigidbody2D.AddForce(transform.up * (LinearThrust * m_Configuration.Acceleration * Time.fixedDeltaTime));
            Rigidbody2D.AddTorque(AngularThrust * m_Configuration.RotationSpeed * Time.fixedDeltaTime);
            
            // Clamp velocity
            Rigidbody2D.linearVelocity = Vector2.ClampMagnitude(Rigidbody2D.linearVelocity, m_Configuration.MaxSpeed);
        }
    }
}
