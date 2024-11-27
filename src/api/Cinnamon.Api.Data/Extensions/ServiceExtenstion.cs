using Cinnamon.Api.Data.Repository;
using Cinnamon.Api.Data.Repository.DbSets;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Extensions;

public static class ServiceExtenstion
{
    public static IServiceCollection ExtendServices(this IServiceCollection services) 
    {
        services.AddTransient(typeof(IGenericEntity<>), typeof(GenericEntity<>));
        services.AddTransient<IActivity, ActivityEntity>();
        services.AddTransient<IActivityAddress, ActivityAddressEntity>();
        services.AddTransient<IActivityDescription, ActivityDescriptionEntity>();
        services.AddTransient<IActivityImage, ActivityImageEntity>();
        services.AddTransient<IActivitySchedule, ActivityScheduleEntity>();
        services.AddTransient<ISearchTags, SearchTagsEntity>();
        services.AddTransient<ICustomer, CustomerEntity>();
        services.AddTransient<IExperienceCategory, ExperienceCategoryEntity>();
        services.AddTransient<IExperienceType, ExperienceTypeEntity>();
        services.AddTransient<IFamilyMember, FamilyMemberEntity>();
        services.AddTransient<IOngoingActivity, OngoingActivityEntity>();
        services.AddTransient<IPurchaseOrder, PurchaseOrderEntity>();
        services.AddTransient<IResendEmail, ResendEmailEntity>();
        services.AddTransient<IWaitList, WaitListEntity>();
        services.AddTransient<IStudent, StudentEntity>();
        services.AddTransient<IStudentAttendance, StudentAttendanceEntity>();
        services.AddTransient<IBadgeList, BadgeListEntity>();
        services.AddTransient<IFailedLogin, FailedLoginEntity>();
        services.AddTransient<IChatRooms, ChatRoomsEntity>();
        services.AddTransient<IChatHistory, ChatHistoryEntity>();
        services.AddTransient<IChatMember, ChatMemberEntity>();
        services.AddTransient<IDataStore, DataStore>();
        services.AddTransient<IReviews, ReviewsEntity>();
        services.AddTransient<IFavorite, FavoriteEntity>();
        services.AddTransient<IChatConnection, ChatConnectionEntity>();
        services.AddTransient<IExperienceCreationType, ExperienceCreationTypeEntity>();
        services.AddTransient<IAddOns, AddOnsEntity>(); 
        services.AddTransient<IOteOnlineEvent, OteOnlineEventEntity>();
        services.AddTransient<IOteWaitlist, OteWaitlistEntity>();
        services.AddTransient<IGuestOTP, GuestOTPEntity>();

        services.AddTransient<Services.Repository.Interfaces.IActivityRepository, Services.Repository.Activity.ActivityRepository>();
        services.AddTransient<Services.Repository.Interfaces.IAddressRepository, Services.Repository.ActivityAddress.AddressRepository>();
        services.AddTransient<Services.Repository.Interfaces.IDescriptionRepository, Services.Repository.ActivityDescription.DescriptionRepository>();
        services.AddTransient<Services.Repository.Interfaces.ICustomerRepository, Services.Repository.Customer.CustomerRepository>();
        services.AddTransient<Services.Repository.Interfaces.IWaitListRepository, Services.Repository.Waitlist.WaitListRepository>();
        services.AddTransient<Services.Repository.Interfaces.IResendEmailRepository, Services.Repository.ResendEmail.ResendEmailRepository>();
        services.AddTransient<Services.Repository.Interfaces.IPurchaseOrderRepository,Services.Repository.PurchaseOrder.PurchaseOrderRepository>();
        services.AddTransient<Services.Repository.Interfaces.IOngoingActivityRepository, Services.Repository.OngoingActivity.OngoingActivityRepository>();
        services.AddTransient<Services.Repository.Interfaces.IFamilyMemberRepository, Services.Repository.FamilyMember.FamilyMemberRepository>();
        services.AddTransient<Services.Repository.Interfaces.IExperienceCategoryRepository, Services.Repository.ExperienceCategory.ExperienceCategoryRepository>();
        services.AddTransient<Services.Repository.Interfaces.ISubCategoryRepository, Services.Repository.ExperienceSubCategory.SubCategoryRepository>();
        services.AddTransient<Services.Repository.Interfaces.IActivityImageRepository, Services.Repository.ActivityImage.ActivityImageRepository>();
        services.AddTransient<Services.Repository.Interfaces.IExperienceTypeRepository, Services.Repository.ExperienceType.ExperienceTypeRepository>();
        services.AddTransient<Services.Repository.Interfaces.IScheduleRepository, Services.Repository.Schedule.ScheduleRepository>();
        services.AddTransient<Services.Repository.Interfaces.ISearchTagsRepository, Services.Repository.SearchTag.SearchTagRepository>();
        services.AddTransient<Services.Repository.Interfaces.IExternalLoginTokenRepository, Services.Repository.ExternalLoginToken.ExternalLoginTokenRepository>();
        services.AddTransient<Services.Repository.Interfaces.IStudentRepository, Services.Repository.Student.StudentRepository>();
        services.AddTransient<Services.Repository.Interfaces.IStudentAttendanceRepository, Services.Repository.StudentAttendance.StudentAttendanceRepository>();
        services.AddTransient<Services.Repository.Interfaces.IResetPasswordRepository, Services.Repository.ResetPassword.ResetPasswordRepository>();
        services.AddTransient<Services.Repository.Interfaces.ILocationRepository, Services.Repository.Location.LocationRepository>();
        services.AddTransient<Services.Repository.Interfaces.IFailedLoginRepository, Services.Repository.FailedLogin.FailedLoginRepository>();
        services.AddTransient<Services.Repository.Interfaces.IRequestRefundRepository, Services.Repository.RequestRefund.RequestRefundRepository>();
        services.AddTransient<Services.Repository.Interfaces.IPayoutAccountRepository, Services.Repository.PayoutAccount.PayoutAccountRepository>();
        services.AddTransient<Services.Repository.Interfaces.IPayoutLogRepository, Services.Repository.PayoutLog.PayoutLogRepository>();
        services.AddTransient<Services.Repository.Interfaces.IAdminUserRepository, Services.Repository.AdminUser.AdminUserRepository>();
        services.AddTransient<Services.Repository.Interfaces.IBadgeListRepository, Services.Repository.BadgeList.BadgeListRepository>();
        services.AddTransient<Services.Repository.Interfaces.ICustomerPricingRepository, Services.Repository.CustomerPricing.CustomerPricingRepository>();
        services.AddTransient<Services.Repository.Interfaces.IChatHistoryRepository, Services.Repository.ChatHistory.ChatHistoryRepository>();
        services.AddTransient<Services.Repository.Interfaces.IChatRoomRepository, Services.Repository.ChatRoom.ChatRoomRepository>();
        services.AddTransient<Services.Repository.Interfaces.IChatMemberRepository, Services.Repository.ChatRoom.ChatMemberRepository>();
        services.AddTransient<Services.Repository.Interfaces.IReviewsRepository, Services.Repository.Reviews.ReviewsRepository>();
        services.AddTransient<Services.Repository.Interfaces.ICouponRepository, Services.Repository.Coupon.CouponRespository>();
        services.AddTransient<Services.Repository.Interfaces.IFavoriteRepository, Services.Repository.Favorite.FavoriteRepository>();
        services.AddTransient<Services.Repository.Interfaces.IChatConnectionRepository, Services.Repository.ChatConnection.ChatConnectionRepository>();
        services.AddTransient<Services.Repository.Interfaces.IExperienceCreationTypeRepository, Services.Repository.ExperienceCreationType.ExperienceCreationTypeRepository>();
        services.AddTransient<Services.Repository.Interfaces.IOteTicketRepository, Services.Repository.OteTicket.OteTicketRepository>();
        services.AddTransient<Services.Repository.Interfaces.ITokenGeneratedRepository, Services.Repository.TokenGenerated.TokenGeneratedRepository>();
        services.AddTransient<Services.Repository.Interfaces.IAddOnsRepository, Services.Repository.AddOns.AddOnsRepository>();
        services.AddTransient<Services.Repository.Interfaces.IOteDateRepository, Services.Repository.OteDate.OteDateRepository>();
        services.AddTransient<Services.Repository.Interfaces.IDisbursementRepository, Services.Repository.Disbursement.DisbursementRepository>();
        services.AddTransient<Services.Repository.Interfaces.IChatUnreadNotificationRepository, Services.Repository.ChatUnreadNotification.ChatUnreadNotificationRepository>();
        services.AddTransient<Services.Repository.Interfaces.IAnnouncementRepository, Services.Repository.Announcement.AnnouncementRepository>();
        services.AddTransient<Services.Repository.Interfaces.IDynamicContnetRepository, Services.Repository.DynamicContent.DynamicContentRepository>();
        services.AddTransient<Services.Repository.Interfaces.IOnlineEventRepository, Services.Repository.OnlineEvent.OnlineEventRepository>();
        services.AddTransient<Services.Repository.Interfaces.IDirectStudentRepository, Services.Repository.DirectStudent.DirectStudentRepository>();
        services.AddTransient<Services.Repository.Interfaces.IDirectStudentAttendanceRepository, Services.Repository.DirectStudent.DirectStudentAttendanceRepository>();
        services.AddTransient<Services.Repository.Interfaces.IDynamicEmailTemplateRepository, Services.Repository.DynamicEmailTemplate.DynamicEmailTemplateRepository>();
        services.AddTransient<Services.Repository.Interfaces.IOteWaitlistRepository, Services.Repository.OteWaitlist.OteWaitlistRepository>();
        services.AddTransient<Services.Repository.Interfaces.IOteReminderRepository, Services.Repository.OteReminderFlag.OteReminderFlagRepository>();
        services.AddTransient<Services.Repository.Interfaces.IProviderCustomQuestionRepository, Services.Repository.ProviderCustomQuestion.ProviderCustomQuestionRepository>();
        services.AddTransient<Services.Repository.Interfaces.ISeatPlanFormatterRepository, Services.Repository.SeatPlan.SeatPlanFormatterRepository>();
        services.AddTransient<Services.Repository.Interfaces.ISeatPlanTemplateRepository, Services.Repository.SeatPlan.SeatPlanTemplateRepository>();
        services.AddTransient<Services.Repository.Interfaces.IGuestOTPRepository, Services.Repository.GuestOTP.GuestOTPRepository>();

        return services;
    }
}