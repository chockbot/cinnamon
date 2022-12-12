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

    public IActivitySearchTag ActivitySearchTag => new ActivitySearchTagEntity(applicationContext);

    public ICustomer Customer => new CustomerEntity(applicationContext);

    public IExperienceCategory ExperienceCategory => new ExperienceCategoryEntity(applicationContext);

    public IExperienceType ExperienceType => new ExperienceTypeEntity(applicationContext);

    public IFamilyMember FamilyMember => new FamilyMemberEntity(applicationContext);

    public IOngoingActivity OngoingActivity => new OngoingActivityEntity(applicationContext);

    public IPurchaseOrder PurchaseOrder => new PurchaseOrderEntity(applicationContext);

    public IResendEmail ResendEmail => new ResendEmailEntity(applicationContext);

    public IWaitList WaitList => new WaitListEntity(applicationContext);

    public ISubCategory SubCategory => new SubCategoryEntity(applicationContext);

    public async Task EnsureMigrate()
    {
        //await applicationContext.Database.MigrateAsync();
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
                CategoryId = 2, 
                Subcategory = "Math"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id          = 2,
                CategoryId  = 2,
                Subcategory = "Science"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id          = 3,
                CategoryId  = 2,
                Subcategory = "Filipino"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id          = 4,
                CategoryId  = 2,
                Subcategory = "Social Studies"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id          = 5,
                CategoryId  = 2,
                Subcategory = "Art"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id          = 6,
                CategoryId  = 2,
                Subcategory = "Music"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id          = 7,
                CategoryId  = 2,
                Subcategory = "English"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 8,
                CategoryId = 2,
                Subcategory = "Araling Panlipunan"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 9,
                CategoryId = 2,
                Subcategory = "Creative Writing"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 10,
                CategoryId = 2,
                Subcategory = "Coding"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 11,
                CategoryId = 2,
                Subcategory = "Reading"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 12,
                CategoryId = 2,
                Subcategory = "Writing"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 13,
                CategoryId = 3,
                Subcategory = "Mandarin"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 14,
                CategoryId = 3,
                Subcategory = "Spanish"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 15,
                CategoryId = 3,
                Subcategory = "Japanese"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 16,
                CategoryId = 3,
                Subcategory = "Tagalog"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 17,
                CategoryId = 3,
                Subcategory = "Korean"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 18,
                CategoryId = 3,
                Subcategory = "English"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 19,
                CategoryId = 4,
                Subcategory = "Piano"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 20,
                CategoryId = 4,
                Subcategory = "Guitar"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 21,
                CategoryId = 4,
                Subcategory = "Drums"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 22,
                CategoryId = 4,
                Subcategory = "Bass"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 23,
                CategoryId = 5,
                Subcategory = "Fun Play"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 24,
                CategoryId = 6,
                Subcategory = "Occupational Theraphy"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 25,
                CategoryId = 6,
                Subcategory = "Swimming Lessons" 
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 26,
                CategoryId = 6,
                Subcategory = "Yoga Lessons"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 27,
                CategoryId = 7,
                Subcategory = "Archery"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 28,
                CategoryId = 7,
                Subcategory = "Baseball"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 29,
                CategoryId = 7,
                Subcategory = "Basketball"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 30,
                CategoryId = 7,
                Subcategory = "Cheerleading"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 31,
                CategoryId = 7,
                Subcategory = "Dance"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 32,
                CategoryId = 7,
                Subcategory = "Equestiran"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 33,
                CategoryId = 7,
                Subcategory = "Field Hockey"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 34,
                CategoryId = 7,
                Subcategory = "Football"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 35,
                CategoryId = 7,
                Subcategory = "Golf"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 36,
                CategoryId = 7,
                Subcategory = "Gymnastics"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 37,
                CategoryId = 7,
                Subcategory = "Ice Hockey"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 38,
                CategoryId = 7,
                Subcategory = "Karate"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 39,
                CategoryId = 7,
                Subcategory = "Lacrosse"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 40,
                CategoryId = 7,
                Subcategory = "Rowing"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 41,
                CategoryId = 7,
                Subcategory = "Snowboarding"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 42,
                CategoryId = 7,
                Subcategory = "Soccer"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 43,
                CategoryId = 7,
                Subcategory = "Surfing"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 44,
                CategoryId = 7,
                Subcategory = "Swimming"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 45,
                CategoryId = 7,
                Subcategory = "Table Tennis"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 46,
                CategoryId = 7,
                Subcategory = "Tennis"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 47,
                CategoryId = 7,
                Subcategory = "Track and Field"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 48,
                CategoryId = 7,
                Subcategory = "Volleyball"
            });
            applicationContext.SubCategory.Add(new Entities.SubCategory
            {
                Id = 49,
                CategoryId = 7,
                Subcategory = "Chess"
            });
        }
        await applicationContext.SaveChangesAsync();
    }
}