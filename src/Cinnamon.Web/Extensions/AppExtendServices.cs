namespace Cinnamon.Web.Extensions;

public static class ExtensionService
{
    public static IServiceCollection AppExtendServices(this IServiceCollection services)
    {
        services.AddTransient<Modules.ApiAccess.Handlers.IAccountApiHandler, Modules.ApiAccess.Account.AccountApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.IActivityApiHandler, Modules.ApiAccess.Activity.ActivityApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.ITransactionApiHandler, Modules.ApiAccess.Transaction.TransactionApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.IOngoingActivitiesHandler, Modules.ApiAccess.OngoingActivities.OnGoingActivityApiHandler>();  
        services.AddTransient<Modules.ApiAccess.Handlers.IDashboardApiHandler, Modules.ApiAccess.Dashboard.DashboardApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.ISystemApiHandler, Modules.ApiAccess.System.SystemApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.IPaymentApiHandler, Modules.ApiAccess.Payment.PaymentApiHandler>();

        return services;
    } 
}