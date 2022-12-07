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

        await applicationContext.SaveChangesAsync();
    }
}