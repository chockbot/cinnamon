using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GetCustomerByHandler : IGetCustomerByHandler
{
    private readonly ICustomerData customerData;

    public GetCustomerByHandler(ICustomerData customerData)
    {
        this.customerData = customerData;
    }

    public AppResult<GetCustomerByHandlerResult> Execute(GetCustomerByHandlerArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerByHandlerResult>.CreateFailed(ex, "An error occured in GetCustomerByIdHandler");
        }
    }

    public async Task<AppResult<GetCustomerByHandlerResult>> ExecuteAsync(GetCustomerByHandlerArgs args)
    {
        try
        {
            var result = await customerData.GetCustomerByHandler(args.Handler);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetCustomerByHandlerResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetCustomerByHandlerResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetCustomerByIdHandler");
            }

            return AppResult<GetCustomerByHandlerResult>.CreateSucceeded(new GetCustomerByHandlerResult
            {
                FirstName = result.Result.Result.FirstName,
                LastName = result.Result.Result.LastName,
                Id = result.Result.Result.Id,
                IsMaker = result.Result.Result.IsMaker,
                IsVerified = result.Result.Result.IsVerified,
                IsOG = result.Result.Result.IsOG,
                IsOfficial = result.Result.Result.IsOfficial,
                ProfileImg = result.Result.Result.ProfileImg,
                About = result.Result.Result.About

            }, "Successfully getting customer information");
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerByHandlerResult>.CreateFailed(ex, "An error occured in GetCustomerByIdHandler");
        }
    }
}