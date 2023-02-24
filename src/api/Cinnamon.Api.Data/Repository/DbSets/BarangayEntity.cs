using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class BarangayEntity: GenericEntity<Barangay>, IBarangay
    {
        public BarangayEntity(ApplicationContext applicationContext):base(applicationContext)
        {

        }
    }
}
