namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IDataStore
{
    IActivity Activity { get; }
    IActivityAddress ActivityAddress { get; }
    IActivityDescription ActivityDescription { get; }
    IActivityImage ActivityImage { get; }
    IActivitySchedule ActivitySchedule { get; }
    ICustomer Customer { get; }
    IExperienceCategory ExperienceCategory { get; }
    IExperienceType ExperienceType { get; }
    IFamilyMember FamilyMember { get; }
    IOngoingActivity OngoingActivity { get; }
    IPurchaseOrder PurchaseOrder { get; }
    IResendEmail ResendEmail { get; }
    IWaitList WaitList { get; }
    ISubCategory SubCategory { get; } 
    ISearchTags SearchTags { get; }
    IExternalLoginToken ExternalLoginToken { get; }
    IStudent Student { get; }
    IStudentAttendance StudentAttendance { get; }
    IResetPassword ResetPassword { get; }
    IRegion Region { get; }
    ICity City { get; }
    IBarangay Barangay{ get; }
    IBadgeList BadgeList { get; }
    IFailedLogin FailedLogin { get; }
    IRequestRefund RequestRefund { get; }
    IPayoutAccount PayoutAccount {get;}
    IPayoutLog PayoutLog {get;}
    IAdminUser AdminUser {get; }
    ICustomerPricing CustomerPricing {get; }
    IChatHistory ChatHistory {get; }
    IChatRooms ChatRooms {get; }
    IChatMember ChatMember {get; }
    IReviews Reviews { get; }
    IFavorite Favorite { get; }
    ICoupon Coupon {get; }
    IChatConnection ChatConnection {get; }
    IActivityScheduleTime ActivityScheduleTime {get; }
    IExperienceCreationType ExperienceCreationType {get; }
    IOngoingActivityScheduleTime OngoingActivityScheduleTime { get; }
    IOteSchedule OteSchedule {get;}
    IOteSchedulePricing OteSchedulePricing {get;}
    IOteSchedulePricingGroup OteSchedulePricingGroup { get; }
    IOteTicket OteTicket {get;}
    ITokenGenerated TokenGenerated {get;}
    IAddOns AddOns { get; }
    IOteDate OteDate {get;}
    IOteOnlineEvent OteOnlineEvent { get; }
    IOteDateOverride OteDateOverride {get;}
    IOteSharedLink OteSharedLink {get;}
    IDisbursement Disbursement {get;}
    IDisbursementDetail DisbursementDetail {get;}
    IDisbursementBulk DisbursementBulk {get;}
    IDisbursementDetailBulk DisbursementDetailBulk {get;}
    IDisbursementBulkLog DisbursementBulkLog {get;}
    IDisbursementManual DisbursementManual {get;}
    IChatUnreadNotification ChatUnreadNotification {get;}
    IAnnouncement Announcement {get;}
    IDynamicContent DynamicContent {get;}
    IActivitySummary ActivitySummary {get;}
    IDynamicEmailTemplate DynamicEmailTemplate {get;}
    IOteWaitlist OteWaitlist { get;}
    
    Task EnsureMigrate();

    Task SeedData();
}