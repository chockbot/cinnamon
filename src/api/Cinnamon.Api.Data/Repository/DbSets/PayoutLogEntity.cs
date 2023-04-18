using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class PayoutLogEntity : GenericEntity<PayoutLog>, IPayoutLog 
{
    public PayoutLogEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}