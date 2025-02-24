using Managers;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class BootstrapperScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Register managers
            builder.RegisterEntryPoint<Bootstrapper>(Lifetime.Scoped);
        }
    }
}