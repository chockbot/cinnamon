using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ExperienceCategoryEntity : GenericEntity<ExperienceCategory>, IExperienceCategory
{
    public ExperienceCategoryEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}