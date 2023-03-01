using Cinnamon.Api.Core.Modules;

namespace Cinnamon.Api.Core.Extensions;

public static class ServiceExtenstion 
{
    public static IServiceCollection ExtendServices(this IServiceCollection services)
    {
        services.AddTransient<Providers.IContainerProvider, Providers.ContainerProvider>();
        services.AddTransient<Providers.IJsonSerializationProvider, Providers.DefaultJsonSerialization>();

        // low level modules
        services.AddTransient<Modules.EmailDriver.Handlers.ISendMailHandler, Modules.EmailDriver.MicrosoftGraph.SendMailByMicrosoftGraph>();
        services.AddTransient<Modules.NotificationDriver.Handler.ISendVerifyEmailHandler, Modules.NotificationDriver.EmailNotification.SendVerifyEmailHandler>();
        services.AddTransient<Modules.UploadDriver.Handlers.IUploadAzureBlob, Modules.UploadDriver.AzureBlob.UploadAzureBlobHandler>();
        services.AddTransient<Modules.UploadDriver.Handlers.IDeleteAzureBlob, Modules.UploadDriver.AzureBlob.DeleteAzureBlobHandler>();
        services.AddTransient<Modules.NotificationDriver.Handler.ICustomerPayedNotificationHandler, Modules.NotificationDriver.EmailNotification.CustomerPayedNotificationHandler>();
        services.AddTransient<Modules.NotificationDriver.Handler.IMakerEnrolledNotificationHandler, Modules.NotificationDriver.EmailNotification.MakerEnrolledNotificationHandler>();
        services.AddTransient<Modules.NotificationDriver.Handler.IResetPasswordNotificationHandler, Modules.NotificationDriver.EmailNotification.ResetPasswordNotificationHandler>();
        services.AddTransient<Modules.NotificationDriver.Handler.IVerifyResetPasswordNotificationHandler, Modules.NotificationDriver.EmailNotification.VerifyResetPasswordNotificationHandler>();

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
        services.AddTransient<Modules.DataAccess.Handlers.IExternalLoginTokenData, Modules.DataAccess.ExternalLoginToken.ExternalLoginTokenData>();
        services.AddTransient<Modules.DataAccess.Handlers.IOngoingActivitiesData, Modules.DataAccess.OngoingActivities.OngoingActivitiesData>();
        services.AddTransient<Modules.DataAccess.Handlers.IStudentData, Modules.DataAccess.Student.StudentData>();
        services.AddTransient<Modules.DataAccess.Handlers.IStudentAttendanceData, Modules.DataAccess.StudentAttendance.StudentAttendanceData>();
        services.AddTransient<Modules.DataAccess.Handlers.IResetPasswordData, Modules.DataAccess.ResetPassword.ResetPasswordData>();
        services.AddTransient<Modules.DataAccess.Handlers.IRegionData, Modules.DataAccess.Location.RegionData>();
        services.AddTransient<Modules.DataAccess.Handlers.ICityData, Modules.DataAccess.Location.CityData>();
        services.AddTransient<Modules.DataAccess.Handlers.IBarangayData, Modules.DataAccess.Location.BarangayData>();
        services.AddTransient<Modules.DataAccess.Handlers.IFailedLoginData, Modules.DataAccess.FailedLogin.FailedLoginData>();

        // ongoing activity services
        services.AddTransient<Services.OngoingActivityService.Handlers.ICreateOngoingActivityHandler, Services.OngoingActivityService.CreateOngoingActivityHandler>();

        // account services
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
        services.AddTransient<Services.AccountService.Handlers.IGetCustomerByIdHandler, Services.AccountService.GetCustomerByIdHandler>();
        services.AddTransient<Services.AccountService.Handlers.IExternalLoginHandler, Services.AccountService.SubmitExternalLoginHandler>();
        services.AddTransient<Services.AccountService.Handlers.IExternalRegisterHandler, Services.AccountService.SubmitExternalRegisterHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGetExternalLoginDetailHandler, Services.AccountService.GetExternalLoginDetailHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGetCustomerByHandler, Services.AccountService.GetCustomerByHandler>();
        services.AddTransient<Services.AccountService.Handlers.IResetPasswordHandler, Services.AccountService.ResetPasswordHandler>();
        services.AddTransient<Services.AccountService.Handlers.IVerifyResetPasswordHandler, Services.AccountService.VerifyResetPasswordHandler>();
        services.AddTransient<Services.AccountService.Handlers.IBannedAccountHandler, Services.AccountService.BannedAccountHandler>();
        
        // activity services
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
        services.AddTransient<Services.ActivityService.Handlers.IGetActivitiesBySubCategoriesHandler, Services.ActivityService.GetActivitiesBySubCategoriesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetEnrolledActivitiesHandler, Services.ActivityService.GetEnrolledActivitiesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IUpdateActivityImageOrderHandler, Services.ActivityService.UpdateActivityImageOrderHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetActivityByHandler, Services.ActivityService.GetActivityByHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetOwnedActivityByHandler, Services.ActivityService.GetOwnedActivityByHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetAllRegionsHandler, Services.ActivityService.GetAllRegionsHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetAllCitiesHandler, Services.ActivityService.GetAllCitiesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetAllBarangaysHandler, Services.ActivityService.GetAllBarangaysHandler>();

        // transaction services
        services.AddTransient<Services.TransactionService.Handlers.IPurchaseOrderHandler, Services.TransactionService.PurchaseOrderHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IGetPurchaseOrderHandler, Services.TransactionService.GetPurchaseOrderHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IRequestPaymentHandler, Services.TransactionService.RequestPaymentHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IFinishTransactionHandler, Services.TransactionService.FinishTransactionHandler>();

        // dashboard services
        services.AddTransient<Services.DashboardService.Handlers.IGetActivitySchedulesHandler, Services.DashboardService.GetActivityScheduleHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetStudentAttendanceHandler, Services.DashboardService.GetStudentAttendanceHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetCurrentDateAttendanceHandler, Services.DashboardService.GetCurrentDateAttendanceHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IUpdateStudentAttendanceHandler, Services.DashboardService.UpdateStudentAttendanceHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IUpdateStudentAttendanceCurrentDateHandler, Services.DashboardService.UpdateStudentAttendanceCurrentDateHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IUpdateAttendanceHandler, Services.DashboardService.UpdateAttendanceHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetAllStudentAttendanceByIdHandler,Services.DashboardService.GetAllStudentAttendanceByIdHandler>();
        services.AddTransient<Services.DashboardService.Handlers.ICreateStudentAttendanceHandler, Services.DashboardService.CreateStudentAttendanceHandler>();

        //OnGoingActivities
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetAllOngoingActivitiesHandler, Services.OnGoingActivityService.GetAllOngoingActivitiesHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetOngoingActivityByIdHandler, Services.OnGoingActivityService.GetOngoingActivityByIdHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IUpdateOngoingActivityHadler, Services.OnGoingActivityService.UpdateOngoingActivityHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IAddActivityExpirationHandler, Services.OnGoingActivityService.AddActivityExpirationHandler>();

        //system
        services.AddTransient<Services.SystemService.Handlers.IGetSystemDateHandler, Services.SystemService.GetSystemDateHandler>();

        // payment gateways
        services.AddTransient<Services.PaymentGatewayService.Zendit.EWalletGenerateResponseHandler>();
        services.AddTransient<Services.PaymentGatewayService.Handlers.IVerifyCallbackHandler, Services.PaymentGatewayService.Zendit.VerifyCallbackHandler>();
        
        return services;
    }
}