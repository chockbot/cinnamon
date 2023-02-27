using Microsoft.EntityFrameworkCore;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Repository.DbSets;

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
        }
            await applicationContext.SaveChangesAsync();
    }
}