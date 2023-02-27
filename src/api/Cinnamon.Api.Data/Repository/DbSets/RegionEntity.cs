using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class RegionEntity : GenericEntity<Region>, IRegion
    {
        public RegionEntity(ApplicationContext applicationContext)
            : base(applicationContext)
        {
        }
    }
}
