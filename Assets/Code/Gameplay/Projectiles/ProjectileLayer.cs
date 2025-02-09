using System;

namespace Gameplay.Projectiles
{
    [Flags]
    public enum ProjectileLayer
    {
        None = 0,
        Player = 1 << 0,
        Asteroid = 1 << 1,
        Enemy  = 1 << 2,
        
        Obstacles = Asteroid | Enemy,
        All = Player | Asteroid | Enemy
    }
}