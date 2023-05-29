using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class CustomerPricingEntity : GenericEntity<CustomerPricing> , ICustomerPricing 
{
    private readonly ApplicationContext applicationContext;

    public CustomerPricingEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}