using Managers.Menu;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class MenuScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Register managers
            builder.RegisterComponentInHierarchy<MenuManager>();
        }
    }
}