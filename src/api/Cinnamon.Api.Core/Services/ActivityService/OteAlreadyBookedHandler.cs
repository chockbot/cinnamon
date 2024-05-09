using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteAlreadyBookedHandler : IOteAlreadyBookedHandler
{
    private readonly IActivityData activityData;
    private readonly IMapper mapper;
    private readonly IGetProfileHandler profileHandler;
    private readonly IGetActivityHandler getActivityHandler;

    public OteAlreadyBookedHandler(IActivityData activityData, IMapper mapper, 
        IGetProfileHandler profileHandler, IGetActivityHandler getActivityHandler)
    {
        this.activityData = activityData;
        this.mapper = mapper;
        this.profileHandler = profileHandler;
        this.getActivityHandler = getActivityHandler;
    }
    
    public AppResult<OteAlreadyBookedResult> Execute(OteAlreadyBookedArgs args)
    {   
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteAlreadyBookedResult>> ExecuteAsync(OteAlreadyBookedArgs args)
    {
        try
        {
            var customerRes = await profileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!customerRes.Succeeded || customerRes.Result is null)
            {
                return AppResult<OteAlreadyBookedResult>.CreateFailed(new ApplicationException(customerRes.Message), customerRes.Message);
            }

            var activityRes = await getActivityHandler.ExecuteAsync(new GetActivityArgs {
                ActivityId = args.ActivityId,
                IncludeCustomer = true
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<OteAlreadyBookedResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }

            if(activityRes.Result.Owner?.Id != customerRes.Result.Id)
            {
                return AppResult<OteAlreadyBookedResult>.CreateFailed(
                    new ApplicationException("Invalid request. Action not allowed."), "Invalid request. Action not allowed.");
            }

            var alreadyBookedRes = await activityData.OteAlreadyBooked(args.ActivityId);
            if(!alreadyBookedRes.Succeeded || alreadyBookedRes.Result is null || !alreadyBookedRes.Result.IsSuccess)
            {
                return AppResult<OteAlreadyBookedResult>.CreateFailed(
                    new ApplicationException(alreadyBookedRes.Result?.ErrorInfo?.Message), alreadyBookedRes.Message);
            }

            var result = mapper.Map<IEnumerable<OteAlreadyBookedResult.OteAlreadyBooked>>(alreadyBookedRes.Result.Result);
            return AppResult<OteAlreadyBookedResult>.CreateSucceeded(
                new OteAlreadyBookedResult { OteAlreadyBookedItems = result}, "Successfully get ote already booked dates.");
        }
        catch (Exception ex)
        {
            return AppResult<OteAlreadyBookedResult>.CreateFailed(ex, "An error occured in OteAlreadyBookedHandler.");
        }
    }
}