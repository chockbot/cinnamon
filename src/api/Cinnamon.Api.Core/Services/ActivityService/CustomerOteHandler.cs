using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class CustomerOteHandler : ICustomerOteHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IActivityData activityData;
    private readonly IMapper mapper;
    
    public CustomerOteHandler(IGetProfileHandler getProfileHandler, IActivityData activityData,
        IMapper mapper)
    {
        this.getProfileHandler = getProfileHandler;
        this.activityData = activityData;
        this.mapper = mapper;
    }

    public AppResult<CustomerOteResult> Execute(CustomerOteArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CustomerOteResult>.CreateFailed(ex, "An error occured in CustomerOteHandler");
        }
    }

    public async Task<AppResult<CustomerOteResult>> ExecuteAsync(CustomerOteArgs args)
    {
        try
        {
            var customerLogin = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!customerLogin.Succeeded || customerLogin.Result is null)
            {
                return AppResult<CustomerOteResult>.CreateFailed(new ApplicationException(customerLogin.Message), customerLogin.Message);
            }

            var customerOteRes = await activityData.CustomerOte(customerLogin.Result.Id);
            if(!customerOteRes.Succeeded || customerOteRes.Result is null || !customerOteRes.Result.IsSuccess)
            {
                return AppResult<CustomerOteResult>.CreateFailed(new ApplicationException(customerOteRes.Message), customerOteRes.Message);
            }

            var mapResult = mapper.Map<IEnumerable<CustomerOteResult.CustomerOte>>(customerOteRes.Result.Result);
            return AppResult<CustomerOteResult>.CreateSucceeded(new CustomerOteResult{CustomerOtes = mapResult}, "Successfully get customer ote"); 
        }
        catch (Exception ex)
        {
            return AppResult<CustomerOteResult>.CreateFailed(ex, "An error occured in CustomerOteHandler");
        }
    }
}