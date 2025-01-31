using System.Collections.Generic;
using Other;
using UnityEngine;

namespace Gameplay.UnboundedSpace
{
    public class UnboundedSpaceBehaviour : MonoBehaviour
    {
        [SerializeField] private Camera                         m_Camera;
        private readonly         List<IUnboundedSpaceTransform> m_Objects = new();
        
        public void Register(IUnboundedSpaceTransform obj) => m_Objects.Add(obj);
        public void Unregister(IUnboundedSpaceTransform obj) => m_Objects.Remove(obj);
        
        private void FixedUpdate()
        {
            Bounds2D bounds = GetViewportBounds();
            
            foreach (IUnboundedSpaceTransform obj in m_Objects)
            {
                Vector2 position = obj.Position;
                
                // Check if the object is within the bounds of the viewport
                if (bounds.Intersects(obj.Bounds))
                    continue;
                
                // Teleport the object to the opposite side of the viewport
                if (position.x < bounds.Min.x) position.x      = bounds.Max.x;
                else if (position.x > bounds.Max.x) position.x = bounds.Min.x;
                
                if (position.y < bounds.Min.y) position.y      = bounds.Max.y;
                else if (position.y > bounds.Max.y) position.y = bounds.Min.y;
                
                // Update the object's position
                obj.Position = position;
            }
        }
        private Bounds2D GetViewportBounds()
        {
            Vector2 max = m_Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            Vector2 min = m_Camera.ScreenToWorldPoint(Vector3.zero);
            
            return new Bounds2D(min, max);
        }
    }
}