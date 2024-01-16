using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteSchedulePricingGroupsEntity : GenericEntity<OteSchedulePricingGroup>, IOteSchedulePricingGroup
{
    public OteSchedulePricingGroupsEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}