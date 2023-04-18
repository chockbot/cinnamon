using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class PayoutAccountEntity : GenericEntity<PayoutAccount>, IPayoutAccount 
{
    public PayoutAccountEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}