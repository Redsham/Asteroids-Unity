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
        #region Fields

        public bool IsControllable { get; set; } = true;
        
        public Rigidbody2D           Rigidbody2D    { get; private set; }
        public SpriteRenderer        SpriteRenderer { get; private set; }
        public MovementConfiguration Configuration  => m_Configuration;

        public float Rotation
        {
            get => Rigidbody2D.rotation;
            set => Rigidbody2D.rotation = value;
        }
        
        public float LinearThrust
        {
            get => IsControllable ? m_LinearThrust : 0.0f;
            set => m_LinearThrust = Mathf.Clamp(value, 0.0f, 1.0f);
        }
        private float m_LinearThrust;
        
        public float AngularThrust
        {
            get => IsControllable ? m_AngularThrust : 0.0f;
            set => m_AngularThrust = Mathf.Clamp(value, -1.0f, 1.0f);
        }
        private float m_AngularThrust;
        
        public Vector2 Position
        {
            get => Rigidbody2D.position;
            set => Rigidbody2D.position = value;
        }
        public Vector2 Velocity
        {
            get => Rigidbody2D.linearVelocity;
            set => Rigidbody2D.linearVelocity = value;
        }
        public Bounds2D Bounds => Bounds2D.FromSprite(SpriteRenderer);
        
        [SerializeField] private MovementConfiguration m_Configuration;
        
        [Inject] private readonly UnboundedSpaceManager m_UnboundedSpace;

        #endregion


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
            if (IsControllable)
            {
                // Drag
                Velocity = Vector2.Lerp(Rigidbody2D.linearVelocity, Vector2.zero, m_Configuration.LinearDrag * Time.fixedDeltaTime);
                Rigidbody2D.angularVelocity = Mathf.Lerp(Rigidbody2D.angularVelocity, 0.0f, m_Configuration.AngularDrag * Time.fixedDeltaTime);
            
                // Thrust
                Rigidbody2D.AddForce(transform.up * (LinearThrust * m_Configuration.Acceleration * Time.fixedDeltaTime));
                Rigidbody2D.AddTorque(AngularThrust * m_Configuration.RotationSpeed * Time.fixedDeltaTime);
            }
            
            // Clamp velocity
            Velocity = Vector2.ClampMagnitude(Rigidbody2D.linearVelocity, m_Configuration.MaxSpeed);
        }
    }
}
