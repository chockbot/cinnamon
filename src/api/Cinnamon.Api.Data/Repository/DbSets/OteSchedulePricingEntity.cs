using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteSchedulePricingEntity : GenericEntity<OteSchedulePricing>, IOteSchedulePricing
{
    public OteSchedulePricingEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}