using VContainer;

namespace Gameplay.Player.Inputs
{
    public abstract class Input
    {
        [Inject] protected PlayerMovement Movement;
        [Inject] protected PlayerGunner   Gunner;
    }
}