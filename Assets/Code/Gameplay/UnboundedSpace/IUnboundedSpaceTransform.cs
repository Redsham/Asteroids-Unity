using Other;
using UnityEngine;

namespace Gameplay.UnboundedSpace
{
    public interface IUnboundedSpaceTransform
    {
        Vector2 Position { get; set; }
        Bounds2D Bounds { get; }
    }
}