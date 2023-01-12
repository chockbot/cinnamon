using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ExperienceTypeEntity : GenericEntity<ExperienceType>, IExperienceType
{
    public ExperienceTypeEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}