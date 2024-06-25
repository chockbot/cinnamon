using Microsoft.EntityFrameworkCore;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Repository.DbSets;
using Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Repository;

public class DataStore : IDataStore
{
    private readonly ApplicationContext applicationContext;

    public DataStore(ApplicationContext applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public IActivity Activity => new ActivityEntity(applicationContext);

    public IActivityAddress ActivityAddress => new ActivityAddressEntity(applicationContext);

    public IActivityDescription ActivityDescription => new ActivityDescriptionEntity(applicationContext);

    public IActivityImage ActivityImage => new ActivityImageEntity(applicationContext);

    public IActivitySchedule ActivitySchedule => new ActivityScheduleEntity(applicationContext);

    public ICustomer Customer => new CustomerEntity(applicationContext);

    public IExperienceCategory ExperienceCategory => new ExperienceCategoryEntity(applicationContext);

    public IExperienceType ExperienceType => new ExperienceTypeEntity(applicationContext);

    public IFamilyMember FamilyMember => new FamilyMemberEntity(applicationContext);

    public IOngoingActivity OngoingActivity => new OngoingActivityEntity(applicationContext);

    public IPurchaseOrder PurchaseOrder => new PurchaseOrderEntity(applicationContext);

    public IResendEmail ResendEmail => new ResendEmailEntity(applicationContext);

    public IWaitList WaitList => new WaitListEntity(applicationContext);

    public ISubCategory SubCategory => new SubCategoryEntity(applicationContext);

    public ISearchTags SearchTags => new SearchTagsEntity(applicationContext);

    public IExternalLoginToken ExternalLoginToken => new ExternalLoginTokenEntity(applicationContext);

    public IStudent Student => new StudentEntity(applicationContext);

    public IStudentAttendance StudentAttendance => new StudentAttendanceEntity(applicationContext);

    public IResetPassword ResetPassword => new ResetPasswordEntity(applicationContext);

    public IRegion Region => new RegionEntity(applicationContext);

    public ICity City => new CityEntity(applicationContext);

    public IBarangay Barangay => new BarangayEntity(applicationContext);

    public IFailedLogin FailedLogin => new FailedLoginEntity(applicationContext);

    public IRequestRefund RequestRefund => new RequestRefundEntity(applicationContext);

    public IPayoutAccount PayoutAccount => new PayoutAccountEntity(applicationContext);

    public IPayoutLog PayoutLog => new PayoutLogEntity(applicationContext);

    public IBadgeList BadgeList => new BadgeListEntity(applicationContext);

    public IAdminUser AdminUser => new AdminUserEntity(applicationContext);
    
    public ICoupon Coupon => new CouponEntity(applicationContext);

    public ICustomerPricing CustomerPricing => new CustomerPricingEntity(applicationContext);
    public IReviews Reviews => new ReviewsEntity(applicationContext);

    public IChatHistory ChatHistory => new ChatHistoryEntity(applicationContext);

    public IChatRooms ChatRooms => new ChatRoomsEntity(applicationContext);

    public IChatMember ChatMember => new ChatMemberEntity(applicationContext);

    public IFavorite Favorite => new FavoriteEntity(applicationContext);

    public IChatConnection ChatConnection => new ChatConnectionEntity(applicationContext);

    public IActivityScheduleTime ActivityScheduleTime => new ActivityScheduleTimeEntity(applicationContext);

    public IExperienceCreationType ExperienceCreationType => new ExperienceCreationTypeEntity(applicationContext);

    public IOngoingActivityScheduleTime OngoingActivityScheduleTime => new OngoingActivityScheduleTimeEntity(applicationContext);
    
    public IOteSchedule OteSchedule => new OteScheduleEntity(applicationContext);

    public IOteSchedulePricing OteSchedulePricing => new OteSchedulePricingEntity(applicationContext);

    public IOteTicket OteTicket => new OteTicketEntity(applicationContext);

    public IOteOnlineEvent OteOnlineEvent => new OteOnlineEventEntity(applicationContext);

    public ITokenGenerated TokenGenerated => new TokenGenratedEntity(applicationContext);

    public IAddOns AddOns => new AddOnsEntity(applicationContext);

    public IOteDate OteDate => new OteDateEntity(applicationContext);
    public IOteSchedulePricingGroup OteSchedulePricingGroup => new OteSchedulePricingGroupsEntity(applicationContext);
    public IOteDateOverride OteDateOverride => new OteDateOverrideEntity(applicationContext);
    public IOteSharedLink OteSharedLink => new OteSharedLinkEntity(applicationContext);
    
    public IDisbursement Disbursement => new DisbursementEntity(applicationContext);
    public IDisbursementDetail DisbursementDetail => new DisbursementDetailEntity(applicationContext);
    public IDisbursementBulk DisbursementBulk => new DisbursementBulkEntity(applicationContext);
    public IDisbursementDetailBulk DisbursementDetailBulk => new DisbursementDetailBulkEntity(applicationContext);
    public IDisbursementBulkLog DisbursementBulkLog => new DisbursementBulkLogEntity(applicationContext);
    public IDisbursementManual DisbursementManual => new DisbursementManualEntity(applicationContext);

    public IChatUnreadNotification ChatUnreadNotification => new ChatUnreadNotificationEntity(applicationContext);

    public IAnnouncement Announcement => new AnnouncementEntity(applicationContext);

    public IDynamicContent DynamicContent => new DynamicContentEntity(applicationContext);


    public IActivitySummary ActivitySummary => new ActivitySummaryEntity(applicationContext);

    public IOteWaitlist OteWaitlist => new OteWaitlistEntity(applicationContext);

    public async Task EnsureMigrate()
    {
        await applicationContext.Database.MigrateAsync();
    }

    public async Task SeedData()
    {
        var experienceTypes = await applicationContext.ExperienceTypes.FirstOrDefaultAsync();
        if (experienceTypes == null)
        {
            applicationContext.ExperienceTypes.Add(new Entities.ExperienceType { Id = 1, Name = "In-Person" });
            applicationContext.ExperienceTypes.Add(new Entities.ExperienceType { Id = 2, Name = "Online" });
        }

        var experienceCategory = await applicationContext.ExperienceCategories.FirstOrDefaultAsync();
        if(experienceCategory == null)
        {
            applicationContext.ExperienceCategories.Add(new Entities.ExperienceCategory
            {
                Id = 1,
                Category = "New",
                IconPath = "images/Category/New.png"
            });

            applicationContext.ExperienceCategories.Add(new Entities.ExperienceCategory
            {
                Id = 2,
                Category = "Academics",
                IconPath = "images/Category/Academic.png"
            });

            applicationContext.ExperienceCategories.Add(new Entities.ExperienceCategory
            {
                Id = 3,
                Category = "Language",
                IconPath = "images/Category/Language.png"
            });

            applicationContext.ExperienceCategories.Add(new Entities.ExperienceCategory
            {
                Id = 4,
                Category = "Music",
                IconPath = "images/Category/Music.png"
            });

            applicationContext.ExperienceCategories.Add(new Entities.ExperienceCategory
            {
                Id = 5,
                Category = "Skill",
                IconPath = "images/Category/Skill.png"
            });

            applicationContext.ExperienceCategories.Add(new Entities.ExperienceCategory
            {
                Id = 6,
                Category = "SPED",
                IconPath = "images/Category/SPED.png"
            });

            applicationContext.ExperienceCategories.Add(new Entities.ExperienceCategory
            {
                Id = 7,
                Category = "Sports",
                IconPath = "images/Category/Sports.png"
            });
        }

        var experienceSubCategory = await applicationContext.SubCategory.FirstOrDefaultAsync();
        if (experienceSubCategory == null)
        {
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 1,
                CatergoryId= 2,
                SubCatergory = "Math"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 2,
                CatergoryId= 2,
                SubCatergory = "Science"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 3,
                CatergoryId = 2,
                SubCatergory = "Filipino"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 4,
                CatergoryId = 2,
                SubCatergory = "Social Studies"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 5,
                CatergoryId = 2,
                SubCatergory = "Art"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 6,
                CatergoryId = 2,
                SubCatergory = "Music"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 7,
                CatergoryId = 2,
                SubCatergory = "English"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 8,
                CatergoryId = 2,
                SubCatergory = "Araling Panlipunan"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 9,
                CatergoryId = 2,
                SubCatergory = "Creative Writing"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 10,
                CatergoryId = 2,
                SubCatergory = "Coding"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 11,
                CatergoryId = 2,
                SubCatergory = "Reading"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 12,
                CatergoryId = 2,
                SubCatergory = "Writing"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 13,
                CatergoryId = 3,
                SubCatergory = "Mandarin"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 14,
                CatergoryId = 3,
                SubCatergory = "Spanish"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 15,
                CatergoryId = 3,
                SubCatergory = "Japanese"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 16,
                CatergoryId = 3,
                SubCatergory = "Tagalog"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 17,
                CatergoryId = 3,
                SubCatergory = "Korean"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 18,
                CatergoryId = 3,
                SubCatergory = "English"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 19,
                CatergoryId = 4,
                SubCatergory = "Piano"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 20,
                CatergoryId = 4,
                SubCatergory = "Guitar"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 21,
                CatergoryId = 4,
                SubCatergory = "Drums"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 22,
                CatergoryId = 4,
                SubCatergory = "Bass"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 23,
                CatergoryId = 5,
                SubCatergory = "Fun Play"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 24,
                CatergoryId = 6,
                SubCatergory = "Occupational Theraphy"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 25,
                CatergoryId = 6,
                SubCatergory = "Swimming Lessons"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 26,
                CatergoryId = 6,
                SubCatergory = "Yoga Lessons"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 27,
                CatergoryId = 7,
                SubCatergory = "Archery"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 28,
                CatergoryId = 7,
                SubCatergory = "Baseball"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 29,
                CatergoryId = 7,
                SubCatergory = "Basketball"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 30,
                CatergoryId = 7,
                SubCatergory = "Cheerleading"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 31,
                CatergoryId = 7,
                SubCatergory = "Dance"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 32,
                CatergoryId = 7,
                SubCatergory = "Equestiran"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 33,
                CatergoryId = 7,
                SubCatergory = "Field Hockey"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 34,
                CatergoryId = 7,
                SubCatergory = "Football"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 35,
                CatergoryId = 7,
                SubCatergory = "Golf"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 36,
                CatergoryId = 7,
                SubCatergory = "Gymnastics"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 37,
                CatergoryId = 7,
                SubCatergory = "Ice Hockey"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 38,
                CatergoryId = 7,
                SubCatergory = "Karate"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 39,
                CatergoryId = 7,
                SubCatergory = "Lacrosse"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 40,
                CatergoryId = 7,
                SubCatergory = "Rowing"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 41,
                CatergoryId = 7,
                SubCatergory = "Snowboarding"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 42,
                CatergoryId = 7,
                SubCatergory = "Soccer"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 43,
                CatergoryId = 7,
                SubCatergory = "Surfing"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 44,
                CatergoryId = 7,
                SubCatergory = "Swimming"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 45,
                CatergoryId = 7,
                SubCatergory = "Table Tennis"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 46,
                CatergoryId = 7,
                SubCatergory = "Tennis"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 47,
                CatergoryId = 7,
                SubCatergory = "Track and Field"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 48,
                CatergoryId = 7,
                SubCatergory = "Volleyball"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 49,
                CatergoryId = 7,
                SubCatergory = "Chess"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 50,
                CatergoryId = 7,
                SubCatergory = "Fencing"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 51,
                CatergoryId = 2,
                SubCatergory = "Others"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 52,
                CatergoryId = 3,
                SubCatergory = "Others"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 53,
                CatergoryId = 4,
                SubCatergory = "Others"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 54,
                CatergoryId = 5,
                SubCatergory = "Others"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 55,
                CatergoryId = 6,
                SubCatergory = "Others"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 56,
                CatergoryId = 7,
                SubCatergory = "Others"
            });
        }

        var makerBadge = await applicationContext.BadgeList.FirstOrDefaultAsync();
        if (makerBadge == null)
        {
            applicationContext.BadgeList.Add(new Entities.BadgeList
            {
                Id = 1,
                Name = "Rare Activity Provider",
                Description = "Completed \"10\" number of students.",
                NumberOfCompletedStudent = 10,
                NumberOfEnrolledStudent = 0,
                NumberOfReviews = 0,
                ImgScr = "images/Badges/RARE.svg"
            });
            applicationContext.BadgeList.Add(new Entities.BadgeList
            {
                Id = 2,
                Name = "Epic Activity Provider",
                Description = "Completed \"25\" number of students and receive \"10\" reviews.",
                NumberOfCompletedStudent = 25,
                NumberOfEnrolledStudent = 0,
                NumberOfReviews = 10,
                ImgScr = "images/Badges/EPIC.svg"
            });
            applicationContext.BadgeList.Add(new Entities.BadgeList
            {
                Id = 3,
                Name = "Legendary Activity Provider",
                Description = "Completed \"100\" number of students and receive \"25\" reviews or more.",
                NumberOfCompletedStudent = 100,
                NumberOfEnrolledStudent = 0,
                NumberOfReviews = 25,
                ImgScr = "images/Badges/LEGENDARY.svg"
            });
            applicationContext.BadgeList.Add(new Entities.BadgeList
            {
                Id = 4,
                Name = "High Demand",
                Description = "First 50 sign-ups enrolled in his/her experience.",
                NumberOfCompletedStudent = 0,
                NumberOfEnrolledStudent = 50,
                NumberOfReviews = 0,
                ImgScr = "images/Badges/HIGH DEMAND.svg"
            });
            applicationContext.BadgeList.Add(new Entities.BadgeList
            {
                Id = 5,
                Name = "Favorite Partner",
                Description = "5 students enrolled more than once.",
                NumberOfCompletedStudent = 0,
                NumberOfEnrolledStudent = 0,
                NumberOfReviews = 0,
                ImgScr = "images/Badges/FAVORITE PARTNER.svg"
            });
            applicationContext.BadgeList.Add(new Entities.BadgeList
            {
                Id = 6,
                Name = "Idea Generator",
                Description = "Sent 5 feedbacks or suggestions.",
                NumberOfCompletedStudent = 0,
                NumberOfEnrolledStudent = 0,
                NumberOfReviews = 0,
                ImgScr = "images/Badges/IDEA GENERATOR.svg"
            });
            applicationContext.BadgeList.Add(new Entities.BadgeList
            {
                Id = 7,
                Name = "Viral Profile",
                Description = "One of the most visited profile in the platform.",
                NumberOfCompletedStudent = 0,
                NumberOfEnrolledStudent = 0,
                NumberOfReviews = 0,
                ImgScr = "images/Badges/VIRAL PROFILE.svg"
            });
        }

        var experienceCreationType = await applicationContext.ExperienceCreationTypes.FirstOrDefaultAsync();
        if (experienceCreationType == null)
        {
            applicationContext.ExperienceCreationTypes.Add(new Entities.ExperienceCreationType
            {
                Name = "General Experiences",
                ImagePath = "/images/experience-creation/general-experience.svg",
                IsActive = true,
                Description = "Great for creating experiences (e.g., Piano Lessons for kids, Swimming Class, etc.)"
            });
            applicationContext.ExperienceCreationTypes.Add(new Entities.ExperienceCreationType
            {
                Name = "Experience via Appointment",
                ImagePath = "/images/experience-creation/appointment.svg",
                IsActive = true,
                Description = "If you want customers to book your experience by appointment"
            });
            applicationContext.ExperienceCreationTypes.Add(new Entities.ExperienceCreationType
            {
                Name = "One Time Events",
                ImagePath = "/images/experience-creation/one-time-event.svg",
                IsActive = true,
                Description = "Great for events like visiting a museum, an educational place, etc."
            });
        }

            await applicationContext.SaveChangesAsync();
    }
}