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
        services.AddTransient<Modules.DataAccess.Handlers.IExperienceCategoryData, Modules.DataAccess.ExperienceCategory.CategoryData>();
        services.AddTransient<Modules.DataAccess.Handlers.ISubCategoryData, Modules.DataAccess.SubCategory.SubCategoryData>();
        services.AddTransient<Modules.DataAccess.Handlers.IActivityImagesData, Modules.DataAccess.ActivityImages.ActivityImagesData>();
        services.AddTransient<Modules.DataAccess.Handlers.IPurchaseOrderData, Modules.DataAccess.PurchaseOrder.PurchaseOrderData>();

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
        services.AddTransient<Services.AccountService.Handlers.IGetProfilePictureHandler,Services.AccountService.GetProfilePictureHandler>();
        services.AddTransient<Services.AccountService.Handlers.IUploadGovernmentIdHandler, Services.AccountService.SubmitUploadGovernmentHandler>();
        services.AddTransient<Services.AccountService.Handlers.IUploadProfilePictureHandler, Services.AccountService.SubmitUploadProfilePictureHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGetWaitListHandler, Services.AccountService.GetWaitListHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGetCustomerByEmailHandler, Services.AccountService.GetCustomerByEmailHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGetWaitListByGuidHandler, Services.AccountService.GetWaitListByGuidHandler>();
        
        
        services.AddTransient<Services.ActivityService.Handlers.ICreateActivityHandler, Services.ActivityService.CreateActivityHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetExperienceTypesHandler, Services.ActivityService.GetExperienceTypesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetExperienceCategoriesHandler, Services.ActivityService.GetExperienceCategoriesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetSubCategoriesHandler, Services.ActivityService.GetSubCategoriesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetOwnedActivitiesHandler, Services.ActivityService.GetOwnedActivitiesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IUpdateActivityHandler, Services.ActivityService.UpdateActivityHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetAllActivitiesHandler,Services.ActivityService.GetAllActivitiesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetOwnedActivityHandler, Services.ActivityService.GetOwnedActivityHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IUploadActivityImageHandler, Services.ActivityService.UploadActivityImageHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetActivityHandler, Services.ActivityService.GetActivityHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetAddressHandler, Services.ActivityService.GetAddressHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetActivityImagesHandler, Services.ActivityService.GetActivityImagesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetActiviesByCategoriesHandler, Services.ActivityService.GetActivitiesByCategoriesHandler>();

        services.AddTransient<Services.TransactionService.Handlers.IPurchaseOrderHandler, Services.TransactionService.PurchaseOrderHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IGetPurchaseOrderHandler, Services.TransactionService.GetPurchaseOrderHandler>();

        return services;
    }
}