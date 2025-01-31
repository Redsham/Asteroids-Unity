using Other;
using UnityEngine;

namespace Gameplay
{
    public interface IUnboundedSpaceTransform
    {
        Vector2 Position { get; set; }
        Bounds2D Bounds { get; }
    }
}