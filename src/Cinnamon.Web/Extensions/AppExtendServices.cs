namespace Cinnamon.Web.Extensions;

public static class ExtensionService
{
    public static IServiceCollection AppExtendServices(this IServiceCollection services)
    {
        services.AddTransient<Modules.ApiAccess.Handlers.IAccountApiHandler, Modules.ApiAccess.Account.AccountApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.IActivityApiHandler, Modules.ApiAccess.Activity.ActivityApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.ITransactionApiHandler, Modules.ApiAccess.Transaction.TransactionApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.IOngoingActivitiesHandler, Modules.ApiAccess.OngoingActivities.OnGoingActivityApiHandler>();  

        return services;
    } 
}