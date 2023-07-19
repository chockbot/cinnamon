using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class CustomerPricingEntity : GenericEntity<CustomerPricing> , ICustomerPricing 
{
    private readonly ApplicationContext applicationContext;

    public CustomerPricingEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<CustomerPricing>>> GetAllowedCustomers(int? take = 100, int? skip = 0)
    {
        try
        {
            int limitCount = take.HasValue ? take.Value : int.MaxValue;
            int skipCount = skip.HasValue ? skip.Value : 0;

            var query = applicationContext.CustomerPricings.
                Include(c => c.Customer).
                Where(c => c.Customer.IsOfficialPartner).
                Skip(skipCount).
                Take(limitCount);
                
            var result = await query.ToListAsync();
            
            return AppResult<IEnumerable<CustomerPricing>>.CreateSucceeded(result, "Successfully get allowed customers pricing");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<CustomerPricing>>.CreateFailed(ex, "An error occured when getting allowed customer pricing");
        }
    }
}