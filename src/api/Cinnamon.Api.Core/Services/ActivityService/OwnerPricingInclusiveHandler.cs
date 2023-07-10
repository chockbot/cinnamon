using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OwnerPricingInclusiveHandler : IOwnerPricingInclusiveHandler
{
    private readonly IGetCustomerByIdHandler getCustomerByIdHandler;
    private readonly ICustomerPricingData customerPricingData;

    public OwnerPricingInclusiveHandler(IGetCustomerByIdHandler getCustomerByIdHandler, ICustomerPricingData customerPricingData)
    {
        this.getCustomerByIdHandler = getCustomerByIdHandler;
        this.customerPricingData = customerPricingData;
    }

    public AppResult<OwnerPricingInclusiveResult> Execute(OwnerPricingInclusiveArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<OwnerPricingInclusiveResult>.CreateFailed(ex, "An error occured in OwnerPricingInclusiveHandler");
        }
    }

    public async Task<AppResult<OwnerPricingInclusiveResult>> ExecuteAsync(OwnerPricingInclusiveArgs args)
    {
        try
        {
            // check if customer is official partner
            var customerRes = await getCustomerByIdHandler.ExecuteAsync(new AccountService.Interactors.GetCustomerByIdArgs {Id = args.CustomerId});
            if(!customerRes.Succeeded || customerRes.Result == null)
            {
                return AppResult<OwnerPricingInclusiveResult>.CreateFailed(new ApplicationException(customerRes.Message), customerRes.Message);
            }
            var customer = customerRes.Result;

            if(!customer.IsOfficial)
            {
                return AppResult<OwnerPricingInclusiveResult>.CreateSucceeded(new OwnerPricingInclusiveResult {IsInclusivePricing = false}, "Request successfuly identified");
            }

            // check customer pricing data
            var customerPricingRes = await customerPricingData.GetCustomerPricingByCustomerId(args.CustomerId);
            if(!customerPricingRes.Succeeded || customerPricingRes.Result == null || !customerPricingRes.Result.IsSuccess)
            {
                return AppResult<OwnerPricingInclusiveResult>.CreateSucceeded(new OwnerPricingInclusiveResult {IsInclusivePricing = false}, "Request successfuly identified");
            }

            if(customerPricingRes.Result.Result.Rate <= 0)
            {
                return AppResult<OwnerPricingInclusiveResult>.CreateSucceeded(new OwnerPricingInclusiveResult {IsInclusivePricing = false}, "Request successfuly identified");
            }

            return AppResult<OwnerPricingInclusiveResult>.CreateSucceeded(new OwnerPricingInclusiveResult {IsInclusivePricing = true}, "Request successfuly identified");
        }
        catch (Exception ex)
        {
            return AppResult<OwnerPricingInclusiveResult>.CreateFailed(ex, "An error occured in OwnerPricingInclusiveHandler");
        }
    }
}