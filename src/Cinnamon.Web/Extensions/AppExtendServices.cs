namespace Cinnamon.Web.Extensions;

public static class ExtensionService
{
    public static IServiceCollection AppExtendServices(this IServiceCollection services)
    {
        services.AddTransient<Modules.ApiAccess.Handlers.IAccountApiHandler, Modules.ApiAccess.Account.AccountApiHandler>();

        return services;
    } 
}