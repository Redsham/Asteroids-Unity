namespace Gameplay.Projectiles
{
    public interface IProjectileCollision
    {
        ProjectileLayer ProjectileLayer { get; }
        void            OnProjectileCollision(ProjectileCollisionData collision);
    }
}