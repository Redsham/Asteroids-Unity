using Gameplay.Asteroids;
using Gameplay.Cameras;
using Gameplay.Enemies;
using Gameplay.Player;
using Gameplay.Player.Inputs;
using Gameplay.Projectiles;
using Gameplay.UnboundedSpace;
using Managers.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Input = Gameplay.Player.Inputs.Input;

namespace Infrastructure
{
    public class GameplayScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Register camera controller
            builder.RegisterComponentInHierarchy<ICameraController>();
            
            // Register unbound space
            builder.RegisterEntryPoint<UnboundedSpaceManager>().AsSelf();
            
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
            if (Application.isMobilePlatform)
                builder.RegisterEntryPoint<MobileInput>().As<Input>();
            else
                builder.RegisterEntryPoint<DesktopInput>().As<Input>();
            
            // Register managers
            builder.RegisterEntryPoint<ScoreManager>().AsSelf();
            builder.RegisterEntryPoint<WavesManager>().AsSelf();
            builder.RegisterEntryPoint<GameManager>().AsSelf();
        }
    }
}