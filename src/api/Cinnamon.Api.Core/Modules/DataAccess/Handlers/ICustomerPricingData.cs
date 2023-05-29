using Cinnamon.Framework.ApiCommand.ApiData.CustomerPricing.Request;
using Cinnamon.Framework.ApiCommand.ApiData.CustomerPricing.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface ICustomerPricingData
{
    Task<AppResult<GetCustomerPricingResult>> GetCustomerPricingById(int id);
    Task<AppResult<GetCustomerPricingResult>> GetCustomerPricingByCustomerId(int id);
    Task<AppResult<GetAllCustomerPricingResult>> GetAllCustomerPricing(GetAllCustomerPricingArgs args);
    Task<AppResult<CreateCustomerPricingResult>> CreateCustomerPricing(CreateCustomerPricingArgs args);
    Task<AppResult<UpdateCustomerPricingResult>> UpdateCustomerPricing(UpdateCustomerPricingArgs args);
}