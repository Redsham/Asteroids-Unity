namespace Gameplay.Asteroids
{
    public interface IAsteroidCollisionHandler
    {
        void OnAsteroidCollision(AsteroidBehaviour asteroid);
    }
}