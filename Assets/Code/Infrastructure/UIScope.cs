using VContainer;
using VContainer.Unity;
using UI.Gameplay;

namespace Infrastructure
{
    public class UIScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Register UI components
            builder.RegisterComponentInHierarchy<ScoreDrawer>();
            builder.RegisterComponentInHierarchy<LivesDrawer>();
        }
    }
}