using UnityEngine;

namespace Other
{
    public struct Bounds2D
    {
        public Bounds2D(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }
        
        
        public Vector2 Min;
        public Vector2 Max;
        
        public Vector2 Center => (Min + Max) / 2;
        public Vector2 Size => Max - Min;
        
        public float Width => Size.x;
        public float Height => Size.y;
        
        
        public bool Contains(Vector2 point)
        {
            return point.x >= Min.x && point.x <= Max.x && point.y >= Min.y && point.y <= Max.y;
        }
        public bool Intersects(Bounds2D other)
        {
            return Min.x <= other.Max.x && Max.x >= other.Min.x && Min.y <= other.Max.y && Max.y >= other.Min.y;
        }
        
        public Vector2 ClosestPoint(Vector2 point)
        {
            float x = Mathf.Clamp(point.x, Min.x, Max.x);
            float y = Mathf.Clamp(point.y, Min.y, Max.y);
            
            return new Vector2(x, y);
        }
        public Vector2 RandomPoint()
        {
            return new Vector2(Random.Range(Min.x, Max.x), Random.Range(Min.y, Max.y));
        }
        public Vector2 RandomPointOnEdge()
        {
            float x = Random.Range(Min.x, Max.x);
            float y = Random.Range(Min.y, Max.y);
            
            return Random.value < 0.5f ? new Vector2(x, Random.value < 0.5f ? Min.y : Max.y) : new Vector2(Random.value < 0.5f ? Min.x : Max.x, y);
        }
        
        public Bounds2D Expand(Vector2 amount)
        {
            return new Bounds2D(Min - amount, Max + amount);
        }
        public Bounds2D Expand(float amount) => Expand(new Vector2(amount, amount));
        
        public Bounds2D Translate(Vector2 amount)
        {
            return new Bounds2D(Min + amount, Max + amount);
        }
        public Bounds2D Translate(float x, float y) => Translate(new Vector2(x, y));
        
        public static Bounds2D FromCenter(Vector2 center, Vector2 size)
        {
            Vector2 halfSize = size / 2;
            return new Bounds2D(center - halfSize, center + halfSize);
        }
        public static Bounds2D FromCenter(Vector2 center, float width, float height) => FromCenter(center, new Vector2(width, height));
        public static Bounds2D FromPoint(Vector2 point) => new(point, point);
        public static Bounds2D FromPoint(Vector2 point, float radius) => new(point - new Vector2(radius, radius), point + new Vector2(radius, radius));
    }
}