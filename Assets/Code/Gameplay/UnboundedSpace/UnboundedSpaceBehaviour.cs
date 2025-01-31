using System.Collections.Generic;
using Other;
using UnityEngine;

namespace Gameplay
{
    public class UnboundedSpaceBehaviour : MonoBehaviour
    {
        [SerializeField] private Camera                          m_Camera;
        private                  List<IUnboundedSpaceTransform> m_Objects = new();
        
        public void Register(IUnboundedSpaceTransform obj)
        {
            m_Objects.Add(obj);
        }
        public void Unregister(IUnboundedSpaceTransform obj)
        {
            m_Objects.Remove(obj);
        }
        
        private void FixedUpdate()
        {
            Bounds2D bounds = GetViewportBounds();
            
            foreach (IUnboundedSpaceTransform obj in m_Objects)
            {
                Vector2 position = obj.Position;
                
                // Check if the object is within the bounds of the viewport
                if (bounds.Intersects(obj.Bounds))
                    continue;
                
                // Calculate the closest
                Vector2 closest = bounds.ClosestPoint(bounds.Center - position);
                obj.Position = closest;
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