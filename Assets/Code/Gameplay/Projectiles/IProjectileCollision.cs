namespace Gameplay.Projectiles
{
    public interface IProjectileCollision
    {
        void OnProjectileCollision(ProjectileCollisionData collision);
    }
}