using Cinnamon.Api.Core.Modules;

namespace Cinnamon.Api.Core.Extensions;

public static class ServiceExtenstion 
{
    public static IServiceCollection ExtendServices(this IServiceCollection services)
    {
        // low level modules
        services.AddTransient<Modules.EmailDriver.Handlers.ISendMailHandler, Modules.EmailDriver.MicrosoftGraph.SendMailByMicrosoftGraph>();
        services.AddTransient<Modules.NotificationDriver.Handler.ISendVerifyEmailHandler, Modules.NotificationDriver.EmailNotification.SendVerifyEmailHandler>();
        services.AddTransient<Modules.UploadDriver.Handlers.IUploadAzureBlob, Modules.UploadDriver.AzureBlob.UploadAzureBlobHandler>();
        services.AddTransient<Modules.UploadDriver.Handlers.IDeleteAzureBlob, Modules.UploadDriver.AzureBlob.DeleteAzureBlobHandler>();

        // data access modules
        services.AddTransient<Modules.DataAccess.Handlers.ICustomerData, Modules.DataAccess.Customer.CustomerData>();
        services.AddTransient<Modules.DataAccess.Handlers.IAddressData, Modules.DataAccess.Address.AddressData>();
        services.AddTransient<Modules.DataAccess.Handlers.IWaitListData, Modules.DataAccess.Waitlist.WaitlistData>();
        services.AddTransient<Modules.DataAccess.Handlers.IResendEmailData, Modules.DataAccess.ResendEmail.ResendEmailData>();
        services.AddTransient<Modules.DataAccess.Handlers.IFamilyMemberData, Modules.DataAccess.FamilyMember.FamilyMemberData>();
        services.AddTransient<Modules.DataAccess.Handlers.IActivityData, Modules.DataAccess.Activity.ActivityData>();
        services.AddTransient<Modules.DataAccess.Handlers.IScheduleData, Modules.DataAccess.Schedule.ScheduleData>();
        services.AddTransient<Modules.DataAccess.Handlers.IExperienceTypeData, Modules.DataAccess.ExperienceType.ExperienceTypeData>();

        // services
        services.AddTransient<Services.AccountService.Handlers.ISubmitRegisterHandler, Services.AccountService.SubmitRegisterHandler>();
        services.AddTransient<Services.AccountService.Handlers.ISubmitWaitlistHandler, Services.AccountService.SubmitWaitlistHandler>();
        services.AddTransient<Services.AccountService.Handlers.ISubmitVerifyEmailHandler, Services.AccountService.SubmitVerifyEmailHandler>();
        services.AddTransient<Services.AccountService.Handlers.ISubmitLoginHandler, Services.AccountService.SubmitLoginHandler>();
        services.AddTransient<Services.AccountService.Handlers.ISubmitResendVerificationHandler, Services.AccountService.SubmitResendVerificationHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGetProfileHandler, Services.AccountService.GetProfileHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGetFamilyMembersHandler, Services.AccountService.GetFamilyMemberHandler>();
        services.AddTransient<Services.AccountService.Handlers.IUpdateFamilyMembersHandler, Services.AccountService.UpdateFamilyMembersHandler>();
        services.AddTransient<Services.AccountService.Handlers.ICreateFamilyMembersHandler, Services.AccountService.CreateFamilyMembersHandler>();
        services.AddTransient<Services.AccountService.Handlers.IDeleteFamilyMembersHandler, Services.AccountService.DeleteFamilyMembersHandler>();
        services.AddTransient<Services.AccountService.Handlers.ISubmitUpdateProfileHandler, Services.AccountService.SubmitUpdateProfileHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGetGovernmentIdsHandler, Services.AccountService.GetGovernmentIdsHandler>();
        services.AddTransient<Services.AccountService.Handlers.IUploadGovernmentIdHandler, Services.AccountService.SubmitUploadGovernmentHandler>();
        services.AddTransient<Services.ActivityService.Handlers.ICreateActivityHandler, Services.ActivityService.CreateActivityHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetExperienceTypesHandler, Services.ActivityService.GetExperienceTypesHandler>();

        return services;
    }
}