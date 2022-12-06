using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ActivityAddressEntity : GenericEntity<ActivityAddress>, IActivityAddress
{
    public ActivityAddressEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}