using UnityEngine;

namespace Gameplay.Player
{
    [System.Serializable]
    public struct MovementConfiguration
    {
        public float MaxSpeed => m_MaxSpeed;
        public float Acceleration => m_Acceleration;
        public float RotationSpeed => m_RotationSpeed;
        
        public float Deceleration => m_Deceleration;
        public float RotationDeceleration => m_RotationDeceleration;
        
        
        [SerializeField] private float m_MaxSpeed;
        [SerializeField] private float m_Acceleration;
        [SerializeField] private float m_RotationSpeed;
        
        [Space]
        [SerializeField] private float m_Deceleration;
        [SerializeField] private float m_RotationDeceleration;
    }
}