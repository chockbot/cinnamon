namespace Cinnamon.Api.Core.Providers;

public class ContainerProvider : IContainerProvider
{
    private readonly IServiceProvider provider;

    public ContainerProvider(IServiceProvider provider)
    {
        this.provider = provider;
    }

    public object Resolve(Type resolveType)
    {
        return provider.GetRequiredService(resolveType);
    }
}