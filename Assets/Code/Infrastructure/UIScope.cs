using UI.Gameplay;
using UI.Gameplay.Elements;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class UIScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Register UI components
            builder.RegisterComponentInHierarchy<ScoreDrawer>();
            builder.RegisterComponentInHierarchy<LivesDrawer>();
            builder.RegisterComponentInHierarchy<WaveNotify>();
            
            // Register UI screens
            builder.RegisterComponentInHierarchy<GameOverView>();
            builder.RegisterComponentInHierarchy<GameScreensDrawer>();
        }
    }
}