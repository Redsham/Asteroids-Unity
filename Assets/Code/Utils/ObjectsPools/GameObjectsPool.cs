using System.Collections.Generic;
using UnityEngine;

namespace Utils.ObjectsPools
{
    [System.Serializable]
    public class GameObjectsPool<T> where T : MonoBehaviour, IPoollable
    {
        [SerializeField] private T         m_Prefab;
        [SerializeField] private int       m_InitialSize = 4;
        [SerializeField] private Transform m_Parent;

        /// <summary>
        /// Pool of instances
        /// </summary>
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
        /// <summary>
        /// Spawns a new instance of the prefab
        /// </summary>
        /// <returns></returns>
        private T MakeInstance()
        {
            // Instantiate prefab
            T instance = Object.Instantiate(m_Prefab, m_Parent);
            
            // Initialize instance
            if (instance is IPoolInitializer initializer)
                initializer.OnPoolInitialize();
            
            return instance;
        }

        /// <summary>
        /// Returns an instance from the pool
        /// </summary>
        public T Get()
        {
            // Get instance from pool or create new one
            T instance = m_Pool.Count > 0 ? m_Pool.Dequeue() : MakeInstance();
            instance.gameObject.SetActive(true);
            
            // Handle instance get
            if (instance is IPoolGetHandler getHandler)
                getHandler.OnPoolGet();
            
            // Mark instance as in use
            instance.IsUsing = true;
            
            return instance;
        }
        /// <summary>
        /// Returns an instance to the pool
        /// </summary>
        public void Return(T instance)
        {
            // Return instance to pool
            instance.gameObject.SetActive(false);
            m_Pool.Enqueue(instance);
            
            // Handle instance return
            if (instance is IPoolReturnHandler returnHandler)
                returnHandler.OnPoolReturn();
            
            // Mark instance as not in use
            instance.IsUsing = false;
        }
    }
}