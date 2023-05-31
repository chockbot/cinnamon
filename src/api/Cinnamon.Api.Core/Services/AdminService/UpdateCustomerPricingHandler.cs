using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AdminService;

public class UpdateCustomerPricingHandler : IUpdateCustomerPricingHandler
{
    private readonly ICustomerPricingData customerPricingData;
    private readonly IGetCustomerByIdHandler getCustomerByIdHandler;

    public UpdateCustomerPricingHandler(ICustomerPricingData customerPricingData, IGetCustomerByIdHandler getCustomerByIdHandler)
    {
        this.customerPricingData = customerPricingData;
        this.getCustomerByIdHandler = getCustomerByIdHandler;
    }

    public AppResult<UpdateCustomerPricingResult> Execute(UpdateCustomerPricingArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCustomerPricingResult>.CreateFailed(ex, "An error occured in UpdateCustomerPricingHandler");
        }
    }

    public async Task<AppResult<UpdateCustomerPricingResult>> ExecuteAsync(UpdateCustomerPricingArgs args)
    {
        try
        {
            var customerRes = await getCustomerByIdHandler.ExecuteAsync(new GetCustomerByIdArgs {Id = args.CustomerId});
            if(!customerRes.Succeeded || customerRes.Result == null)
            {
                return AppResult<UpdateCustomerPricingResult>.CreateFailed(new ApplicationException(customerRes.Message), customerRes.Message);
            }
            var customer = customerRes.Result;

            if(!customer.IsOfficial) 
            {
                return AppResult<UpdateCustomerPricingResult>.CreateFailed(new ApplicationException("Invalid request"), "Invalid request");
            }

            // check if already have customer pricing data
            var customerPricingRes = await customerPricingData.GetCustomerPricingByCustomerId(args.CustomerId);
            if(!customerPricingRes.Succeeded || customerPricingRes.Result == null)
            {
                return AppResult<UpdateCustomerPricingResult>.CreateFailed(new ApplicationException(customerPricingRes.Message), customerPricingRes.Message);
            }

            if(customerPricingRes.Succeeded && customerPricingRes.Result != null && !customerPricingRes.Result.IsSuccess)
            {
                // dont have yet customer pricing data, then create customer pricing data
                if(customerPricingRes.Result.ErrorInfo?.Message == "Can't find entity")
                {
                    var createCustomerPricingRes = await customerPricingData.CreateCustomerPricing(new Framework.ApiCommand.ApiData.CustomerPricing.Request.CreateCustomerPricingArgs {
                        CustomerId = args.CustomerId,
                        Email = customer.Email,
                        Rate = args.Rate
                    });
                    if(!createCustomerPricingRes.Succeeded || createCustomerPricingRes.Result == null || !createCustomerPricingRes.Result.IsSuccess)
                    {
                        return AppResult<UpdateCustomerPricingResult>.CreateFailed(
                            new ApplicationException(createCustomerPricingRes.Result?.ErrorInfo?.Message), createCustomerPricingRes.Message);
                    }

                    return AppResult<UpdateCustomerPricingResult>.CreateSucceeded(new UpdateCustomerPricingResult {}, "Customer pricing successfully created");
                }

                // other error
                return AppResult<UpdateCustomerPricingResult>.CreateFailed(
                            new ApplicationException(customerPricingRes.Result?.ErrorInfo?.Message), customerPricingRes.Message);
            }
            
            var updateCustomerPricing = await customerPricingData.UpdateCustomerPricing(new Framework.ApiCommand.ApiData.CustomerPricing.Request.UpdateCustomerPricingArgs {
                Id = customerPricingRes.Result?.Result.Id ?? 0,
                Rate = args.Rate
            });
            if(!updateCustomerPricing.Succeeded || updateCustomerPricing.Result == null)
            {
                return AppResult<UpdateCustomerPricingResult>.CreateFailed(new ApplicationException(updateCustomerPricing.Message), updateCustomerPricing.Message);
            }

            return AppResult<UpdateCustomerPricingResult>.CreateSucceeded(new UpdateCustomerPricingResult {}, "Customer Pricing successfully updated");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCustomerPricingResult>.CreateFailed(ex, "An error occured in UpdateCustomerPricingHandler");
        }
    }
}