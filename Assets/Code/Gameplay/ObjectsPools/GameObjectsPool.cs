using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.ObjectsPools
{
    [System.Serializable]
    public class GameObjectsPool<T> where T : MonoBehaviour
    {
        [SerializeField] private T   m_Prefab;
        [SerializeField] private int m_InitialSize = 4;

        private Queue<T> m_Pool;
        
        public void Initialize(Transform parent)
        {
            m_Pool = new Queue<T>(m_InitialSize);

            for (int i = 0; i < m_InitialSize; i++)
            {
                T instance = MakeInstance();
                Return(instance);
            }
        }
        
        private T MakeInstance()
        {
            T instance = Object.Instantiate(m_Prefab);
            
            // Initialize instance
            if (instance is IPoolInitializer initializer)
                initializer.OnPoolInitialize();
            
            return instance;
        }
        
        public T Get()
        {
            T instance = m_Pool.Count > 0 ? m_Pool.Dequeue() : MakeInstance();
            instance.gameObject.SetActive(true);
            
            if (instance is IPoolGetHandler getHandler)
                getHandler.OnPoolGet();
            
            return instance;
        }
        public void Return(T instance)
        {
            instance.gameObject.SetActive(false);
            m_Pool.Enqueue(instance);
            
            if (instance is IPoolReturnHandler returnHandler)
                returnHandler.OnPoolReturn();
        }
    }
}