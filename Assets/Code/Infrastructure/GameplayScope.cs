using Gameplay.Player;
using Gameplay.Player.Inputs;
using Gameplay.Projectiles;
using Gameplay.UnboundedSpace;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Input = Gameplay.Player.Inputs.Input;

namespace Infrastructure
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private bool m_IsMobile;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Register unbound space
            builder.RegisterComponentInHierarchy<UnboundedSpaceBehaviour>();
            
            // Register projectiles manager
            builder.RegisterComponentInHierarchy<ProjectilesManager>();
            
            // Register player
            builder.RegisterComponentInHierarchy<PlayerMovement>();
            builder.RegisterComponentInHierarchy<PlayerGunner>();
            
            // Register player input
            if (m_IsMobile)
                builder.RegisterEntryPoint<MobileInput>().As<Input>();
            else
                builder.RegisterEntryPoint<DefaultInput>().As<Input>();
        }
    }
}