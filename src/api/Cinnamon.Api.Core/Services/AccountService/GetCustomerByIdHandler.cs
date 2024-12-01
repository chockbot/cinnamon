using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GetCustomerByIdHandler : IGetCustomerByIdHandler
{
	private readonly ICustomerData customerData;
	public GetCustomerByIdHandler(ICustomerData customerData)
	{
		this.customerData = customerData;
	}

    public AppResult<GetCustomerByIdResult> Execute(GetCustomerByIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerByIdResult>.CreateFailed(ex, "An error occured in GetCustomerByIdHandler");
        }
    }

    public async Task<AppResult<GetCustomerByIdResult>> ExecuteAsync(GetCustomerByIdArgs args)
    {
        try
        {
            var result = await customerData.GetCustomerById(args.Id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetCustomerByIdResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetCustomerByIdResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetCustomerByIdHandler");
            }

            return AppResult<GetCustomerByIdResult>.CreateSucceeded(new GetCustomerByIdResult
            {
                About        = result.Result.Result.About,
                //Birthdate  = result.Result.Result.Birthdate,
                PhoneNumber  = result.Result.Result.PhoneNumber,
                DateJoined   = result.Result.Result.DateJoined,
                Email        = result.Result.Result.Email,
                FirstName    = result.Result.Result.FirstName,
                LastName     = result.Result.Result.LastName,
                IsMaker      = result.Result.Result.IsMaker,
                IsVerified   = result.Result.Result.IsVerified,
                IsOG         = result.Result.Result.IsOG,
                IsOfficial   = result.Result.Result.IsOfficial,
                ProfileImg   = result.Result.Result.ProfileImg,
                Id           = result.Result.Result.Id,
                ConnectionId = result.Result.Result.ConnectionId,
                Handler      = result.Result.Result.Handler,
                IsGuest      = result.Result.Result.IsGuest,
            }, "Successfully getting customer information");
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerByIdResult>.CreateFailed(ex, "An error occured in GetCustomerByIdHandler");
        }
    }
}