using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class CityEntity: GenericEntity<City>, ICity
    {
        public CityEntity(ApplicationContext applicationContext): base(applicationContext)
        {

        }
    }
}
