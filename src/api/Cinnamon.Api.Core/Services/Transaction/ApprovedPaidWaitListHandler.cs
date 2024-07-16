using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
using Flurl;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class ApprovedPaidWaitListHandler : IApprovedPaidWaitListHandler
{
    private readonly IActivityData activityData;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly ApplicationConfig applicationConfig;

    public ApprovedPaidWaitListHandler(IActivityData activityData, IGetProfileHandler getProfileHandler,
        IJsonSerializationProvider jsonSerializationProvider, IGetActivityHandler getActivityHandler,
        ApplicationConfig applicationConfig)
    {
        this.activityData = activityData;
        this.getProfileHandler = getProfileHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.getActivityHandler = getActivityHandler;
        this.applicationConfig = applicationConfig;
    }
    
    public AppResult<ApprovedPaidWaitListResult> Execute(ApprovedPaidWaitListArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<ApprovedPaidWaitListResult>> ExecuteAsync(ApprovedPaidWaitListArgs args)
    {
        try
        {
            var profileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result is null)
            {
                return AppResult<ApprovedPaidWaitListResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }
            var profile = profileRes.Result;

            var waitListRes = await activityData.GetOteWaitList(args.WaitListId);
            if(!waitListRes.Succeeded || waitListRes.Result is null || !waitListRes.Result.IsSuccess)
            {
                return AppResult<ApprovedPaidWaitListResult>.CreateFailed(new ApplicationException("Unable to get waitlist."), "Unable to get waitlist.");
            }
            var waitlist = waitListRes.Result.Result;

            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = waitlist.ActivityId,
                IncludeCustomer = true
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<ApprovedPaidWaitListResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }
            if(activityRes.Result.Owner?.Id != profile.Id)
            {
                return AppResult<ApprovedPaidWaitListResult>.CreateFailed(
                    new ApplicationException("Invalid selected activity. Invalid request."), "Invalid selected activity. Invalid request.");
            }
            var activity = activityRes.Result;

            // create link for ticket details
            var url = applicationConfig.FrontendUrl
                .AppendPathSegment("explore")
                .AppendPathSegment("ote")
                .AppendPathSegment(activity.Handler);
            
            return AppResult<ApprovedPaidWaitListResult>.CreateSucceeded(new ApprovedPaidWaitListResult {PurchaseLink = url}, "Successfully approved paid wait list.");
        }
        catch (Exception ex)
        {
            return AppResult<ApprovedPaidWaitListResult>.CreateFailed(ex, "An error occured in ApprovedPaidWaitListHandler.");
        }
    }

}