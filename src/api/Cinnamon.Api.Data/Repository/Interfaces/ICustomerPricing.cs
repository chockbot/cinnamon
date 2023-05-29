using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface ICustomerPricing : IGenericEntity<CustomerPricing>
{
    Task<AppResult<IEnumerable<CustomerPricing>>> GetAllowedCustomers(int? take = 100, int? skip = 0);
}