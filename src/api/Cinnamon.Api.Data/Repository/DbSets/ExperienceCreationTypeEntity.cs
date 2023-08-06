using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class ExperienceCreationTypeEntity : GenericEntity<ExperienceCreationType>, IExperienceCreationType
    {
        private readonly ApplicationContext applicationContext;

        public ExperienceCreationTypeEntity(ApplicationContext applicationContext) : base(applicationContext)
        {
            this.applicationContext = applicationContext;
        }
    }
}
