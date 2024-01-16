using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteDateEntity : GenericEntity<OteDate>, IOteDate
{
    public OteDateEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}