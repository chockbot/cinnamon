using Cinnamon.Api.Core.Modules;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;

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
        services.AddTransient<Modules.NotificationDriver.Handler.IExpiringStudentNotificationHandler, Modules.NotificationDriver.EmailNotification.ExpiringStudentNotificationHandler>();
        services.AddTransient<Modules.NotificationDriver.Handler.IOteCustomerPayedNotificationHandler, Modules.NotificationDriver.EmailNotification.OteCustomerPayedNotificationHandler>();
        services.AddTransient<Modules.NotificationDriver.Handler.IChatUnreadNotificationHandler, Modules.NotificationDriver.EmailNotification.ChatUnreadNotificationHandler>();

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
        services.AddTransient<Modules.DataAccess.Handlers.IRequestRefundData, Modules.DataAccess.RequestRefund.RequestRefundData>();
        services.AddTransient<Modules.DataAccess.Handlers.IPayoutAccountData, Modules.DataAccess.PayoutAccount.PayoutAccountData>();
        services.AddTransient<Modules.DataAccess.Handlers.IPayoutLogData, Modules.DataAccess.PayoutLog.PayoutLogData>();
        services.AddTransient<Modules.DataAccess.Handlers.IAdminUserData, Modules.DataAccess.AdminUser.AdminUserData>();
        services.AddTransient<Modules.DataAccess.Handlers.IBadgesData, Modules.DataAccess.Badges.BadgesData>();
        services.AddTransient<Modules.DataAccess.Handlers.ICustomerPricingData, Modules.DataAccess.CustomerPricingData>();
        services.AddTransient<Modules.DataAccess.Handlers.IChatHistoryData, Modules.DataAccess.ChatHistory.ChatHistoryData>();
        services.AddTransient<Modules.DataAccess.Handlers.IChatRoomData, Modules.DataAccess.ChatRoom.ChatRoomData>();
        services.AddTransient<Modules.DataAccess.Handlers.IFavoriteData, Modules.DataAccess.Favorite.FavoriteData>();
        services.AddTransient<Modules.DataAccess.Handlers.IReviewsData, Modules.DataAccess.Reviews.ReviewsData>();  
        services.AddTransient<Modules.DataAccess.Handlers.ICouponData, Modules.DataAccess.Coupon.CouponData>();
        services.AddTransient<Modules.DataAccess.Handlers.IExperienceCreationTypeData, Modules.DataAccess.ExperienceCreationType.ExperienceCreationTypeData>();
        services.AddTransient<Modules.DataAccess.Handlers.IOteTicketData, Modules.DataAccess.OteTicket.OteTicketData>();
        services.AddTransient<Modules.DataAccess.Handlers.ITokenGeneratedData, Modules.DataAccess.TokenGenerated.TokenGeneratedData>();
        services.AddTransient<Modules.DataAccess.Handlers.IAddOnsData, Modules.DataAccess.AddOns.AddOnsData>();
        services.AddTransient<Modules.DataAccess.Handlers.IOteDateData, Modules.DataAccess.OteDate.OTeDateData>();
        services.AddTransient<Modules.DataAccess.Handlers.IAnnouncementData, Modules.DataAccess.Announcement.AnnouncementData>();
        services.AddTransient<Modules.DataAccess.Handlers.IDisbursementData, Modules.DataAccess.Disbursement.DisbursementData>();
        services.AddTransient<Modules.DataAccess.Handlers.IDynamicContentData, Modules.DataAccess.DynamincContent.DynamincContentData>();
        services.AddTransient<Modules.DataAccess.Handlers.IDirectStudentData, Modules.DataAccess.DirectStudent.DirectStudentData>();

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
        services.AddTransient<Services.AccountService.Handlers.IRequestRefundHandler, Services.AccountService.RequestRefundHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGetRequestRefundHandler, Services.AccountService.GetRequestRefundHandler>();
        services.AddTransient<Services.AccountService.Handlers.IDeleteProfilePictureHandler, Services.AccountService.DeleteProfilePictureHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGetPayoutAccountHandler, Services.AccountService.GetPayoutAccountHandler>();
        services.AddTransient<Services.AccountService.Handlers.ICreateUpdatePayoutAccountHandler, Services.AccountService.CreateUpdatePayoutAccountHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGetAllCustomersHandler, Services.AccountService.GetAllCustomersHandler>();
        services.AddTransient<Services.AccountService.Handlers.IUpdateCustomerProfileHandler, Services.AccountService.UpdateCustomerProfileHandler>();
        services.AddTransient<Services.AccountService.Handlers.IUpdateRequestRefundHandler, Services.AccountService.UpdateRequestRefundHandler>();
        services.AddTransient<Services.AccountService.Handlers.IUpdateCreditBalanceHandler, Services.AccountService.UpdateCreditBalanceHandler>();
        services.AddTransient<Services.AccountService.Handlers.IAccountSubmitVerifiedHandler, Services.AccountService.AccountSubmitVerifiedHandler>();
        services.AddTransient<Services.AccountService.Handlers.IUpdateConnectionIdHandler, Services.AccountService.UpdateConnectionIdHandler>();
        services.AddTransient<Services.AccountService.Handlers.IGenerateCustomerHandler, Services.AccountService.GenerateCustomerHandler>();
        services.AddTransient<Services.AccountService.Handlers.IVerifyUserNotificationHandler, Services.AccountService.VerifyUserNotificationHandler>();
        services.AddTransient<Services.AccountService.Handlers.IExpiringStudentsHandler, Services.AccountService.ExpiringStudentsHandler>();

        services.AddTransient<Services.AccountService.Handlers.IBlockedAccountHandler, Services.AccountService.BlockedAccountHandler>();
        services.AddTransient<Services.AccountService.Handlers.IIsAccountBlockedHandler, Services.AccountService.IsAccountBlockedHandler>();
        services.AddTransient<Services.AccountService.Handlers.IExtraLoginHandler, Services.AccountService.ExtraLoginHandler>();
        services.AddTransient<Services.AccountService.Handlers.IChangeEmailHandler, Services.AccountService.ChangeEmailHandler>();
        services.AddTransient<Services.AccountService.Handlers.IDeleteWaitlistHandler, Services.AccountService.DeleteWaitlistHandler>();

        // activity services
        services.AddTransient<Services.ActivityService.Handlers.ICreateActivityHandler, Services.ActivityService.CreateActivityHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetExperienceTypesHandler, Services.ActivityService.GetExperienceTypesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetExperienceCategoriesHandler, Services.ActivityService.GetExperienceCategoriesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetSubCategoriesHandler, Services.ActivityService.GetSubCategoriesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetOwnedActivitiesHandler, Services.ActivityService.GetOwnedActivitiesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetMakerActivitiesHandler, Services.ActivityService.GetMakerActivitiesHandler>();
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
        services.AddTransient<Services.ActivityService.Handlers.IGetPopularActivitiesHandler, Services.ActivityService.GetPopularActivitiesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetRefundableExperienceHandler, Services.ActivityService.GetRefundableExperienceHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IUpdateActivityScheduleHandler, Services.ActivityService.UpdateActivityScheduleHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IUpdateActivityGuidHandler, Services.ActivityService.UpdateActivityGuidHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IDeleteActivityHandler, Services.ActivityService.DeleteActivityHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOwnerPricingInclusiveHandler, Services.ActivityService.OwnerPricingInclusiveHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGenerateActivityHandler, Services.ActivityService.GenerateActivityHandler>();
        services.AddTransient<Services.ActivityService.Handlers.ICreateCouponHandler, Services.ActivityService.CreateCouponHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IProviderCreateCouponHandler, Services.ActivityService.ProviderCreateCouponHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetCouponsHandler, Services.ActivityService.GetCouponsHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IUpdateCouponStatusHandler, Services.ActivityService.UpdateCouponStatusHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IValidateCouponCodeHandler, Services.ActivityService.ValidateCouponCodeHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IUpdateCouponHandler, Services.ActivityService.UpdateCouponHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IRecommendedActivitiesHandler, Services.ActivityService.RecommendedActivitiesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOteCreateSharedLinkHandler, Services.ActivityService.OteCreateSharedLinkHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGenerateEventSharedLinkHandler, Services.ActivityService.GenerateEventSharedLinkHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOteValidateSharedLinkHandler, Services.ActivityService.OteValidateSharedLinkHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOteSharedLinkVerificationHandler, Services.ActivityService.OteSharedLinkVerificationHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IActivityFeedHandler, Services.ActivityService.ActivityFeedHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IBatchSummaryUpdateHandler, Services.ActivityService.BatchSummaryUpdateHandler>();

        services.AddTransient<Services.ActivityService.Handlers.ICreateFavoriteHandler, Services.ActivityService.CreateFavoriteHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IRemoveFavoriteHandler, Services.ActivityService.RemoveFavoriteHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetFavoritesByCustomerHandler, Services.ActivityService.GetFavoritesByCustomerHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IPopularActivitiesHandler, Services.ActivityService.PopularActivitiesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetExperienceCreationTypeHandler, Services.ActivityService.GetExperienceCreationTypeHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IGetActivityScheduleTimesHandler, Services.ActivityService.GetActivityScheduleTimesHandler>();
        services.AddTransient<Services.ActivityService.Handlers.ICreateOngoingActivityScheduleHandler, Services.ActivityService.CreateOngoingActivityScheduleHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IDisabledExpiredEventHandler, Services.ActivityService.DisabledExpiredEventHandler>();

        services.AddTransient<Services.ActivityService.Handlers.IOteCreateHandler, Services.ActivityService.OteCreateHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOteUpdateHandler, Services.ActivityService.OteUpdateHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOteFindByHandler, Services.ActivityService.OteFindByHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOteTicketDetailsHandler, Services.ActivityService.OteTicketDetailsHandler>();
        services.AddTransient<Services.ActivityService.Handlers.ICustomerOteHandler, Services.ActivityService.CustomerOteHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOteVerificationHandler, Services.ActivityService.OteVerificationHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IDeleteAddOnsHandler, Services.ActivityService.DeleteAddOnsHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IDeleteAddOnHandler, Services.ActivityService.DeleteAddOnHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IDeleteOnlineEventHandler, Services.ActivityService.DeleteOnlineEventHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOteUpdateSharedLinkStatusHandler, Services.ActivityService.OteUpdateSharedLinkStatusHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IDeleteTicketHandler, Services.ActivityService.DeleteTicketHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOteAlreadyBookedHandler, Services.ActivityService.OteAlreadyBookedHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOteTicketBookedCountHandler, Services.ActivityService.OteTicketBookedCountHandler>();
        services.AddTransient<Services.ActivityService.Handlers.IOteScheduleDatesHandler, Services.ActivityService.OteScheduleDatesHandler>();

        // transaction services
        services.AddTransient<Services.TransactionService.Handlers.IPurchaseOrderHandler, Services.TransactionService.PurchaseOrderHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IGetPurchaseOrderHandler, Services.TransactionService.GetPurchaseOrderHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IRequestPaymentHandler, Services.TransactionService.RequestPaymentHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IFinishTransactionHandler, Services.TransactionService.FinishTransactionHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IGetGrossSalesByProviderHandler, Services.TransactionService.GetGrossSalesByProviderHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IGetPayoutsByProviderHandler, Services.TransactionService.GetPayoutsByProviderHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IOtePurchaseOrderHandler, Services.TransactionService.OtePurchaseOrderHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IOteFinishTransactionHandler, Services.TransactionService.OteFinishTransactionHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IOtePurchaseOrderDetailsHandler, Services.TransactionService.OtePurchaseOrderDetailsHandler>();
        services.AddTransient<Services.TransactionService.Handlers.ITransactionRedirectionHandler, Services.TransactionService.TransactionRedirectionHandler>();
        services.AddTransient<Services.TransactionService.Handlers.IGetDirectStudentSalesHandler, Services.TransactionService.GetDirectStudentSalesHandler>();

        // dashboard services
        services.AddTransient<Services.DashboardService.Handlers.IGetActivitySchedulesHandler, Services.DashboardService.GetActivityScheduleHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetStudentAttendanceHandler, Services.DashboardService.GetStudentAttendanceHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetCurrentDateAttendanceHandler, Services.DashboardService.GetCurrentDateAttendanceHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IUpdateStudentAttendanceHandler, Services.DashboardService.UpdateStudentAttendanceHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IUpdateStudentAttendanceCurrentDateHandler, Services.DashboardService.UpdateStudentAttendanceCurrentDateHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IUpdateAttendanceHandler, Services.DashboardService.UpdateAttendanceHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetAllStudentAttendanceByIdHandler,Services.DashboardService.GetAllStudentAttendanceByIdHandler>();
        services.AddTransient<Services.DashboardService.Handlers.ICreateStudentAttendanceHandler, Services.DashboardService.CreateStudentAttendanceHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetAllBadgesHandler, Services.DashboardService.GetAllBadgeHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetAllStudentsAttendanceHandler, Services.DashboardService.GetAllStudentsAttendanceHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetCompletedStudentsHandler, Services.DashboardService.GetCompletedStudentsHandler>();    
        services.AddTransient<Services.DashboardService.Handlers.IGetOTEByProviderHandler, Services.DashboardService.GetOTEByProviderHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetOTEByActivityIdHandler, Services.DashboardService.GetOTEByActivityIdHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetTicketDetailsHandler, Services.DashboardService.GetTicketDetailsHandler>();    
        services.AddTransient<Services.DashboardService.Handlers.IUpdateOTETicketHandler, Services.DashboardService.UpdateOTETicketHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetOtePerDayHandler, Services.DashboardService.GetOtePerDayHandler>();
        services.AddTransient<Services.DashboardService.Handlers.IGetEnrolledStudentsByProviderHandler, Services.DashboardService.GetEnrolledStudentsByProviderHandler>();
        services.AddTransient<Services.DashboardService.Handlers.ICreateDirectStudentsHandler, Services.DashboardService.CreateDirectStudentsHandler>();

        //OnGoingActivities
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetAllOngoingActivitiesHandler, Services.OnGoingActivityService.GetAllOngoingActivitiesHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetOngoingActivityByIdHandler, Services.OnGoingActivityService.GetOngoingActivityByIdHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IUpdateOngoingActivityHadler, Services.OnGoingActivityService.UpdateOngoingActivityHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IAddActivityExpirationHandler, Services.OnGoingActivityService.AddActivityExpirationHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetEnrolledStudentsHandler, Services.OnGoingActivityService.GetEnrolledStudentsHandler>();  
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetCompletedStudentsByIdHandler, Services.OnGoingActivityService.GetCompletedStudentsByIdHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.ICreateReviewHandler, Services.OnGoingActivityService.CreateReviewHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetAllStudentsByIdHandler, Services.OnGoingActivityService.GetAllStudentsByIdHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetReviewsByMakerIdHandler, Services.OnGoingActivityService.GetReviewsByMakerIdHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetReviewsByCustomerIdHandler, Services.OnGoingActivityService.GetReviewsByCustomerIdHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetReviewsByActivityIdHandler, Services.OnGoingActivityService.GetReviewsByActivityIdHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetStudentLastAttendanceHandler, Services.OnGoingActivityService.GetStudentLastAttendanceHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetEnrolleeMasterListHandler, Services.OnGoingActivityService.GetEnrolleeMasterListHandler>();
        services.AddTransient<Services.OnGoingActivityService.Handlers.IGetAttendanceByFamilyIdHandler, Services.OnGoingActivityService.GetAttendanceByFamilyIdHandler>();
        
        //system
        services.AddTransient<Services.SystemService.Handlers.IGetSystemDateHandler, Services.SystemService.GetSystemDateHandler>();
        services.AddTransient<Services.SystemService.Handlers.IGenerateSitemapHandler, Services.SystemService.GenerateSitemapHandler>();

        // payment gateways
        services.AddTransient<Services.PaymentGatewayService.Zendit.EWalletGenerateResponseHandler>();
        services.AddTransient<Services.PaymentGatewayService.Zendit.CardGenerateResponseHandler>();
        services.AddTransient<Services.PaymentGatewayService.Handlers.IVerifyCallbackHandler, Services.PaymentGatewayService.Zendit.VerifyCallbackHandler>();
        services.AddTransient<Services.PaymentGatewayService.Handlers.IGetPaymentChannelsHandler, Services.PaymentGatewayService.GetPaymentChannelsHander>();
        services.AddTransient<Services.PaymentGatewayService.Handlers.IGeneratePayoutHandler, Services.PaymentGatewayService.GeneratePayoutHandler>();
        services.AddTransient<Services.PaymentGatewayService.Handlers.IVerifyPayoutCallbackHandler, Services.PaymentGatewayService.VerifyPayoutCallbackHandler>();

        //admin services
        services.AddTransient<Services.AdminService.Handlers.IGetAdminUserByEmailHandler, Services.AdminService.GetAdminUserByEmailHandler>();
        services.AddTransient<Services.AdminService.Handlers.IUpdateCustomerPricingHandler, Services.AdminService.UpdateCustomerPricingHandler>();
        services.AddTransient<Services.AdminService.Handlers.IGetAllInclusiveTransactionHandler, Services.AdminService.GetAllInclusiveTransactionHandler>();
        services.AddTransient<Services.AdminService.Handlers.ICreateCouponHandler, Services.AdminService.CreateCouponHandler>();
        services.AddTransient<Services.AdminService.Handlers.ICreateAnnouncementHandler, Services.AdminService.CreateAnnouncementHandler>();
        services.AddTransient<Services.AdminService.Handlers.IUpdateAnnouncementHandler, Services.AdminService.UpdateAnnouncementHandler>();
        services.AddTransient<Services.AdminService.Handlers.IGetAnnouncementsHandler, Services.AdminService.GetAnnouncementsHandler>();
        services.AddTransient<Services.AdminService.Handlers.IDeleteAnnouncementHandler, Services.AdminService.DeleteAnnouncementHandler>();
        services.AddTransient<Services.AdminService.Handlers.IUpdateDynamicContentHandler, Services.AdminService.UpdateDynamicContentHandler>();
        services.AddTransient<Services.AdminService.Handlers.IGetDynamicContentHandler, Services.AdminService.GetDynamicContentHandler>();

        //chat services
        services.AddTransient<Services.ChatService.Handlers.ICreateChatHistoryHandler, Services.ChatService.CreateChatHistoryHandler>();
        services.AddTransient<Services.ChatService.Handlers.IUpdateChatHistoryHandler, Services.ChatService.UpdateChatHistoryHandler>();
        services.AddTransient<Services.ChatService.Handlers.IGetChatHistoryByChatRoomIdHandler, Services.ChatService.GetChatHistoryByChatRoomIdHandler>();
        services.AddTransient<Services.ChatService.Handlers.ICreateChatRoomHandler, Services.ChatService.CreateChatRoomHandler>();
        services.AddTransient<Services.ChatService.Handlers.IGetChatRoomsByUserIdHandler, Services.ChatService.GetChatRoomsByUserIdHandler>();
        services.AddTransient<Services.ChatService.Handlers.IGetChatMembersByChatRoomIdHandler, Services.ChatService.GetChatMembersByChatRoomIdHandler>();
        services.AddTransient<Services.ChatService.Handlers.IUpdateChatMemberHandler, Services.ChatService.UpdateChatMemberHandler>();
        services.AddTransient<Services.ChatService.Handlers.ICreateChatConnectionHandler, Services.ChatService.CreateChatConnectionHandler>();
        services.AddTransient<Services.ChatService.Handlers.IUpdateChatConnectionHandler, Services.ChatService.UpdateChatConnectionHandler>();
        services.AddTransient<Services.ChatService.Handlers.IGetChatConnectionByCustomerHandler, Services.ChatService.GetChatConnectionByCustomerHandler>();
        services.AddTransient<Services.ChatService.Handlers.INotifyUnreadChatsHandler, Services.ChatService.NotifyUnreadChatsHandler>();
        services.AddTransient<Services.ChatService.Handlers.IRequestMessageHandler, Services.ChatService.RequestMessageHandler>();
        services.AddTransient<Services.ChatService.Handlers.IGetRequestMessageHandler, Services.ChatService.GetRequestMessageHandler>();

        // disbursement
        services.AddTransient<Services.Disbursement.Handlers.IGenerateDisbursement, Services.Disbursement.GenerateDisbursement>();
        services.AddTransient<Services.Disbursement.Handlers.IGenerateDisbursementPayout, Services.Disbursement.GenerateDisbursementPayout>();
        services.AddTransient<Services.Disbursement.Handlers.IGetDisbursements, Services.Disbursement.GetDisbursements>();
        services.AddTransient<Services.Disbursement.Handlers.IGetDisbursementDetails, Services.Disbursement.GetDisbursementDetails>();
        services.AddTransient<Services.Disbursement.Handlers.IManualDisbursement, Services.Disbursement.ManualDisbursement>();
        services.AddTransient<Services.Disbursement.Handlers.IGetDisbursementByProviderId, Services.Disbursement.GetDisbursementByProviderId>();

        // direct students services
        services.AddTransient<Services.DirectStudentService.Handlers.IDirectStudentsInfoHandler, Services.DirectStudentService.DirectStudentsInfoHandler>();
        services.AddTransient<Services.DirectStudentService.Handlers.IUpdateDirectStudentHandler, Services.DirectStudentService.UpdateDirectStudentHandler>();

        return services;
    }
}