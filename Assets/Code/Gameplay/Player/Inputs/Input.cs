using VContainer;

namespace Gameplay.Player.Inputs
{
    public abstract class Input
    {
        [Inject] protected PlayerMovement Movement;
        [Inject] protected PlayerGunner   Gunner;
        
        protected float LinearThrust
        {
            get => Movement.LinearThrust;
            set => Movement.LinearThrust = value;
        }
        protected float AngularThrust
        {
            get => Movement.AngularThrust;
            set => Movement.AngularThrust = value;
        }
        protected bool IsFiring
        {
            get => Gunner.IsFiring;
            set => Gunner.IsFiring = value;
        }
    }
}