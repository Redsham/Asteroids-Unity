using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D           Rigidbody2D           { get; private set; }
        public MovementConfiguration MovementConfiguration => m_MovementConfiguration;

        
        [SerializeField] private MovementConfiguration m_MovementConfiguration;

        
        public void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }
        public void FixedUpdate()
        {
            
        }
    }
}
