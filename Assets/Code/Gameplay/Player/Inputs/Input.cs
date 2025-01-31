using VContainer;

namespace Gameplay.Player.Inputs
{
    public abstract class Input
    {
        [Inject] protected readonly PlayerMovement PlayerMovement;
    }
}