using Cinnamon.Api.Core.Modules;

namespace Cinnamon.Api.Core.Extensions;

public static class ServiceExtenstion 
{
    public static IServiceCollection ExtendServices(this IServiceCollection services)
    {
        // low level modules
        services.AddTransient<Modules.EmailDriver.Handlers.ISendMailHandler, Modules.EmailDriver.MicrosoftGraph.SendMailByMicrosoftGraph>();
        services.AddTransient<Modules.NotificationDriver.Handler.ISendVerifyEmailHandler, Modules.NotificationDriver.EmailNotification.SendVerifyEmailHandler>();

        // data access modules
        services.AddTransient<Modules.DataAccess.Handlers.ICustomerData, Modules.DataAccess.Customer.CustomerData>();
        services.AddTransient<Modules.DataAccess.Handlers.IAddressData, Modules.DataAccess.Address.AddressData>();
        services.AddTransient<Modules.DataAccess.Handlers.IWaitListData, Modules.DataAccess.Waitlist.WaitlistData>();

        // services
        services.AddTransient<Services.AccountService.Handlers.ISubmitRegisterHandler, Services.AccountService.SubmitRegisterHandler>();
        services.AddTransient<Services.AccountService.Handlers.ISubmitWaitlistHandler, Services.AccountService.SubmitWaitlistHandler>();
        services.AddTransient<Services.AccountService.Handlers.ISubmitVerifyEmailHandler, Services.AccountService.SubmitVerifyEmailHandler>();
        services.AddTransient<Services.AccountService.Handlers.ISubmitLoginHandler, Services.AccountService.SubmitLoginHandler>();

        return services;
    }
}