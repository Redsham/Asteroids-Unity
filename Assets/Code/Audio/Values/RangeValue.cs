using System;
using UnityEngine;

namespace Audio.Values
{
    [Serializable]
    public struct RangeValue
    {
        public RangeValue(float min, float max)
        {
            m_Min = min;
            m_Max = max;
        }
        
        
        public float Min => m_Min;
        public float Max => m_Max;
        
        [SerializeField] private float m_Min;
        [SerializeField] private float m_Max;
    }
}