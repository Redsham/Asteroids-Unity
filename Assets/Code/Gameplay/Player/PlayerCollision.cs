using Gameplay.Asteroids;
using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerCollision : MonoBehaviour, IProjectileCollision, IAsteroidCollisionHandler
    {
        public void OnAsteroidCollision(AsteroidBehaviour asteroid)
        {
            Debug.Log("Player hit by asteroid");
        }
        public void OnProjectileCollision(Projectile projectile)
        {
            Debug.Log("Player hit by projectile");
        }
    }
}