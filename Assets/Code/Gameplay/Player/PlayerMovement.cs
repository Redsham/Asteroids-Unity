using System;
using Other;
using UnityEngine;
using VContainer;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour, IUnboundedSpaceTransform
    {
        public Rigidbody2D           Rigidbody2D           { get; private set; }
        public MovementConfiguration MovementConfiguration => m_MovementConfiguration;

        
        public Vector2 Position
        {
            get => Rigidbody2D.position;
            set => Rigidbody2D.MovePosition(value);
        }
        public Bounds2D Bounds => Bounds2D.FromCenter(Rigidbody2D.position, Vector2.one);
        
        
        [SerializeField] private MovementConfiguration m_MovementConfiguration;
        
        [Inject] private UnboundedSpaceBehaviour m_UnboundedSpace;


        
        [Inject]
        public void Construct()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            m_UnboundedSpace.Register(this);
        }
        private void OnDestroy() => m_UnboundedSpace.Unregister(this);

        
        public void Thrust(float throttle)
        {
            throttle = Mathf.Clamp(throttle, 0.0f, 1.0f);
            Vector2 force = transform.up * throttle * m_MovementConfiguration.Acceleration;
            
            Rigidbody2D.AddForce(force);
            Rigidbody2D.linearVelocity = Vector2.ClampMagnitude(Rigidbody2D.linearVelocity, m_MovementConfiguration.MaxSpeed);
        }
        public void Rotate(float throttle)
        {
            throttle = Mathf.Clamp(throttle, -1.0f, 1.0f);
            Rigidbody2D.AddTorque(-throttle * m_MovementConfiguration.RotationSpeed * Time.fixedDeltaTime);
        }
    }
}
