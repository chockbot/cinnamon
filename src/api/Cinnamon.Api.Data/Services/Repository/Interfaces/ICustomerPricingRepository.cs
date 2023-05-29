using Cinnamon.Framework.ApiCommand.ApiData.DTO.CustomerPricing;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface ICustomerPricingRepository 
{
    Task<AppResult<CustomerPricingDTO>> GetByIdAsync(int id);
    Task<AppResult<CustomerPricingDTO>> GetByCustomerIdAsync(int id);
    Task<AppResult<IEnumerable<CustomerPricingDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<CustomerPricingDTO>>> GetAllAsync();
    Task<AppResult<CustomerPricingDTO>> Create(int customerId, string email, decimal rate);
    Task<AppResult<CustomerPricingDTO>> Update(int id, decimal rate);
}