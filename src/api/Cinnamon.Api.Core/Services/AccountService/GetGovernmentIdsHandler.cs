using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GetGovernmentIdsHandler : IGetGovernmentIdsHandler
{
    private readonly ICustomerData customerData;
    private readonly IHttpContextAccessor httpContext;

    public GetGovernmentIdsHandler(ICustomerData customerData, IHttpContextAccessor httpContext)
    {
        this.customerData = customerData;
        this.httpContext = httpContext;
    }

    public AppResult<GetGovernmentIdsResult> Execute(GetGovernmentIdsArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetGovernmentIdsResult>.CreateFailed(ex, "An error occured in GetGovernmentIdsHandler");
        }
    }

    public async Task<AppResult<GetGovernmentIdsResult>> ExecuteAsync(GetGovernmentIdsArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<GetGovernmentIdsResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            var result = await customerData.GetGovernmentIds(id);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetGovernmentIdsResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetGovernmentIdsResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetGovernmentIdsHandler");
            }

            return AppResult<GetGovernmentIdsResult>.CreateSucceeded(new GetGovernmentIdsResult {
                BackImageSrc = result.Result.Result.BackIdImagePath,
                FrontImageSrc = result.Result.Result.FrontIdImagePath
            }, "Successfully getting government ids");
        }
        catch (Exception ex)
        {
            return AppResult<GetGovernmentIdsResult>.CreateFailed(ex, "An error occured in GetGovernmentIdsHandler");
        }
    }
}