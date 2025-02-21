using System.Collections.Generic;
using Other;
using UnityEngine;

namespace Gameplay.Cameras
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        public Camera   Camera   { get; private set; }
        public Vector3  Position { get; set; }
        public Bounds2D View
        {
            get
            {
                Vector2 position = Position;
                Vector2 size     = Camera.ViewportToWorldPoint(Vector2.one) - Camera.ViewportToWorldPoint(Vector2.zero);
                
                return Bounds2D.FromCenter(position, size);
            }
        }
        
        private readonly List<CameraShakeBase> m_Shakes = new();
        
        
        private void Awake()
        {
            Camera = GetComponent<Camera>();
            ICameraController.SetActive(this);
            
            Position = transform.position;
        }
        private void Update()
        {
            if (m_Shakes.Count == 0)
                return;
            
            Vector2 shake = Vector2.zero;
            for (int i = 0; i < m_Shakes.Count; i++)
            {
                // Update shake
                shake += m_Shakes[i].Update(Time.deltaTime);
                
                // Remove shake if it's done
                if (m_Shakes[i].IsDone)
                {
                    m_Shakes.RemoveAt(i);
                    i--;
                }
            }
            
            transform.position = Position + (Vector3)shake;
        }
        private void OnDestroy() => ICameraController.SetActive(null);

        public void     Shake(CameraShakeBase shake) => m_Shakes.Add(shake);
    }
}