using UnityEngine;

namespace Gameplay.Enemies
{
    public interface IEnemyTarget
    {
        Vector2 Position { get; }
        Vector2 Velocity { get; }
    }
}