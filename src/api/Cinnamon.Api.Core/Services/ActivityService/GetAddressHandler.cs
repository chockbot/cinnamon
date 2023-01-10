using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetAddressHandler : IGetAddressHandler
{
    private readonly IAddressData addressData;
	public GetAddressHandler(IAddressData addressData)
	{
		this.addressData = addressData;	
	}

    public AppResult<GetAddressResult> Execute(GetAddressArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetAddressResult>.CreateFailed(ex, "An error occured in GetAddressHandler");
        }
    }

    public async Task<AppResult<GetAddressResult>> ExecuteAsync(GetAddressArgs args)
    {
        try
        {
            var result = await addressData.GetAllAddress(new Framework.ApiCommand.ApiData.Address.Request.GetAllAddressArgs {});
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetAddressResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetAddressResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetExperienceCategoriesHandler");
            }

            return AppResult<GetAddressResult>.CreateSucceeded(new GetAddressResult
            {
                Addresses = result.Result.Result.Select(e => {
                    return new GetAddressResult.Address
                    {
                        Id = e.Id,
                        ActivityId= e.ActivityId,
                        Address1 = e.Address1,
                        Address2 = e.Address2,
                        City = e.City,
                        District = e.District
                    };
                })
            }, "Successfully get experience categories");
        }
        catch (Exception ex)
        {
            return AppResult<GetAddressResult>.CreateFailed(ex, "An error occured in GetExperienceCategoriesHandler");
        }
    }
}
