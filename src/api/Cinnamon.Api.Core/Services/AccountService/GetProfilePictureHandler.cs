using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using System.Security.Claims;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GetProfilePictureHandler : IGetProfilePictureHandler
{
    private readonly ICustomerData customerData;
    private readonly IHttpContextAccessor httpContext;
    public GetProfilePictureHandler(ICustomerData customerData, IHttpContextAccessor httpContext)
    {
        this.customerData = customerData;
        this.httpContext = httpContext;
    }

    public AppResult<GetProfilePictureResult> Execute(GetProfilePictureArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetProfilePictureResult>.CreateFailed(ex, "An error occured in GetProfilePictureHandler");
        }
    }

    public async Task<AppResult<GetProfilePictureResult>> ExecuteAsync(GetProfilePictureArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if (customerId == null)
            {
                return AppResult<GetProfilePictureResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            var result = await customerData.GetProfilePicture(id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetProfilePictureResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetProfilePictureResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetProfilePictureHandler");
            }

            return AppResult<GetProfilePictureResult>.CreateSucceeded(new GetProfilePictureResult
            {
                ProfileImagseSrc = result.Result.Result.ProfileImagePath,
            }, "Successfully getting customer profile picture");
        }
        catch (Exception ex)
        {
            return AppResult<GetProfilePictureResult>.CreateFailed(ex, "An error occured in GetProfilePictureHandler");
        }
    }
}
