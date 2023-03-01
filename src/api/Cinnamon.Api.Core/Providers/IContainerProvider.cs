namespace Cinnamon.Api.Core.Providers;

public interface IContainerProvider 
{
    object Resolve(Type resolveType);
}