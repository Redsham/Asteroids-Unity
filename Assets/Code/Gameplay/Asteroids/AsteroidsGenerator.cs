using UnityEngine;

namespace Gameplay.Asteroids
{
    internal static class AsteroidsGenerator
    {
        private const int VERTEX_COUNT = 12;
        private const float ROUGHNESS = 0.1f;
        
        public static Vector2[] GenerateAsteroidVertices(float radius)
        {
            Vector2[]   vertices  = new Vector2[VERTEX_COUNT];
            const float ANGLE_STEP = 360.0f / VERTEX_COUNT;
            
            for (int i = 0; i < VERTEX_COUNT; i++)
            {
                float angle = i * ANGLE_STEP;
                
                float x = radius * (Mathf.Cos(angle * Mathf.Deg2Rad) + Random.Range(-ROUGHNESS, ROUGHNESS));
                float y = radius * (Mathf.Sin(angle * Mathf.Deg2Rad) + Random.Range(-ROUGHNESS, ROUGHNESS));
                
                vertices[i] = new Vector2(x, y);
            }

            return vertices;
        }
    }
}