using VContainer;

namespace Gameplay.Player.Inputs
{
    public abstract class Input
    {
        [Inject] protected readonly PlayerMovement Movement;
        [Inject] protected readonly PlayerGunner   Gunner;
    }
}