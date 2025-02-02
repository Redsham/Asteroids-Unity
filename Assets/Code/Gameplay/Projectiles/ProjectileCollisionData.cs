using UnityEngine;

namespace Gameplay.Projectiles
{
    public struct ProjectileCollisionData
    {
        public ProjectileCollisionData(Projectile projectile, IProjectileCollision collision, RaycastHit2D hit)
        {
            Projectile = projectile;
            Collision  = collision;
            
            Position = hit.point;
            Normal   = hit.normal;
            
            Ignored = false;
        }
        
        
        public Projectile           Projectile { get; }
        public IProjectileCollision Collision  { get; }
        public Vector2              Position   { get; }
        public Vector2              Normal     { get; }
        public bool                 Ignored    { get; private set; }
        
        
        public void Ignore() => Ignored = true;
    }
}