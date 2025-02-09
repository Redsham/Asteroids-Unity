using System.Collections.Generic;
using Other;
using UnityEngine;

namespace Gameplay.UnboundedSpace
{
    public class UnboundedSpaceManager : MonoBehaviour
    {
        public Bounds2D Bounds { get; private set; }
        
        [SerializeField] private Camera                         m_Camera;
        private readonly         List<IUnboundedSpaceTransform> m_Objects        = new();
        private readonly         Stack<IUnboundedSpaceWrapped>  m_WrappedObjects = new();
        
        
        public void Register(IUnboundedSpaceTransform obj) => m_Objects.Add(obj);
        public void Unregister(IUnboundedSpaceTransform obj) => m_Objects.Remove(obj);
        
        public Vector2 ShortestPath(Vector2 start, Vector2 end)
        {
            float dx = Mathf.Repeat(end.x - start.x + Bounds.Width / 2, Bounds.Width) - Bounds.Width / 2;
            float dy = Mathf.Repeat(end.y - start.y + Bounds.Height / 2, Bounds.Height) - Bounds.Height / 2;
            return new Vector2(dx, dy);
        }

        private void Awake() => Bounds = GetViewportBounds();
        private void FixedUpdate()
        {
            Bounds = GetViewportBounds();
            
            m_WrappedObjects.Clear();
            foreach (IUnboundedSpaceTransform obj in m_Objects)
            {
                Vector2 position = obj.Position;
                
                // Check if the object is within the bounds of the viewport
                if (Bounds.Intersects(obj.Bounds))
                    continue;
                
                // Teleport the object to the opposite side of the viewport
                if (position.x < Bounds.Min.x) position.x      = Bounds.Max.x;
                else if (position.x > Bounds.Max.x) position.x = Bounds.Min.x;
                
                if (position.y < Bounds.Min.y) position.y      = Bounds.Max.y;
                else if (position.y > Bounds.Max.y) position.y = Bounds.Min.y;
                
                // Update the object's position
                obj.Position = position;
                
                if(obj is IUnboundedSpaceWrapped wrapped)
                    m_WrappedObjects.Push(wrapped);
            }
            
            // Notify wrapped objects
            while (m_WrappedObjects.TryPop(out IUnboundedSpaceWrapped wrapped))
                wrapped.OnSpaceWrapped();
        }
        private Bounds2D GetViewportBounds()
        {
            Vector2 max = m_Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            Vector2 min = m_Camera.ScreenToWorldPoint(Vector3.zero);
            
            return new Bounds2D(min, max);
        }
    }
}