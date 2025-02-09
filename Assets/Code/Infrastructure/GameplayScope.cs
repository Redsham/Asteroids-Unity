using Gameplay.Asteroids;
using Gameplay.Enemies;
using Gameplay.Player;
using Gameplay.Player.Inputs;
using Gameplay.Projectiles;
using Gameplay.UnboundedSpace;
using Managers;
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
            builder.RegisterComponentInHierarchy<UnboundedSpaceManager>();
            
            // Register managers
            builder.UseComponents(components =>
            {
                components.AddInHierarchy<ProjectilesManager>();
                components.AddInHierarchy<AsteroidsManager>();
                components.AddInHierarchy<EnemiesManager>();
            });
            
            // Register player
            builder.UseComponents(components =>
            {
                components.AddInHierarchy<PlayerMovement>();
                components.AddInHierarchy<PlayerGunner>();
                components.AddInHierarchy<PlayerBehaviour>();
            });
            
            // Register player input
            if (m_IsMobile)
                builder.RegisterEntryPoint<MobileInput>().As<Input>();
            else
                builder.RegisterEntryPoint<DesktopInput>().As<Input>();
            
            // Register managers
            builder.RegisterEntryPoint<ScoreManager>().AsSelf();
            builder.RegisterEntryPoint<GameManager>().AsSelf();
        }
    }
}