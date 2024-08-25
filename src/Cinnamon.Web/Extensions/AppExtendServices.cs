namespace Cinnamon.Web.Extensions;

public static class ExtensionService
{
    public static IServiceCollection AppExtendServices(this IServiceCollection services)
    {   
        services.AddTransient<Modules.Services.Handlers.IWordPressServicesHandler, Modules.Services.WordPressServices>();
        services.AddTransient<Modules.ApiAccess.Handlers.IAccountApiHandler, Modules.ApiAccess.Account.AccountApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.IActivityApiHandler, Modules.ApiAccess.Activity.ActivityApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.ITransactionApiHandler, Modules.ApiAccess.Transaction.TransactionApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.IOngoingActivitiesHandler, Modules.ApiAccess.OngoingActivities.OnGoingActivityApiHandler>();  
        services.AddTransient<Modules.ApiAccess.Handlers.IDashboardApiHandler, Modules.ApiAccess.Dashboard.DashboardApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.ISystemApiHandler, Modules.ApiAccess.System.SystemApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.IPaymentApiHandler, Modules.ApiAccess.Payment.PaymentApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.IAdminApiHandler, Modules.ApiAccess.Admin.AdminApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.IChatApiHandler, Modules.ApiAccess.Chat.ChatApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.IDirectStudentApiHandler, Modules.ApiAccess.DirectStudent.DirectStudentApiHandler>();
        services.AddTransient<Modules.ApiAccess.Handlers.ISeatPlanApiHandler, Modules.ApiAccess.SeatPlan.SeatPlanApiHandler>();

        services.AddHttpContextAccessor();
        services.AddScoped(sp => sp.GetService<IHttpContextAccessor>().HttpContext?.User);

        return services;
    } 
}