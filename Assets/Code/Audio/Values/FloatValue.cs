using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio.Values
{
    [Serializable]
    public struct FloatValue
    {
        public FloatValue(float defaultValue)
        {
            m_Min = defaultValue;
            m_Max = defaultValue;
            m_UseRandom = false;
        }
        
        
        public float Value => m_UseRandom ? Random.Range(m_Min, m_Max) : m_Min;

        [SerializeField] private float m_Min;
        [SerializeField] private float m_Max;
        [SerializeField] private bool  m_UseRandom;
    }
}