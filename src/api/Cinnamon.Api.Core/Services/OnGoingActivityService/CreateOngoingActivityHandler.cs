using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OngoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OngoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OngoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OngoingActivityService;

public class CreateOngoingActivityHandler : ICreateOngoingActivityHandler
{
    private readonly IActivityData activityData;
    private readonly IOngoingActivitiesData ongoingActivitiesData;

    public CreateOngoingActivityHandler(IActivityData activityData,IOngoingActivitiesData ongoingActivitiesData)
    {
        this.activityData = activityData;
        this.ongoingActivitiesData = ongoingActivitiesData;
    }

    public AppResult<CreateOngoingActivityResult> Execute(CreateOngoingActivityArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateOngoingActivityResult>.CreateFailed(ex, "An error occured in CreateOngoingActivityHandler");
        }
    }

    public async Task<AppResult<CreateOngoingActivityResult>> ExecuteAsync(CreateOngoingActivityArgs args)
    {
        try
        {
            if(args.Students == null || args.Students.Count() == 0)
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(
                    new ApplicationException("Empty enrolled stundent not allowed"), "Empty enrolled stundent not allowed");
            }

            // check activity and schedule if associated
            var activityRes = await activityData.GetActivityById(args.ActivityId, 
                new Framework.ApiCommand.ApiData.Activity.Request.GetActivityArgs {
                IncludeSchedules = true
            });
            if(!activityRes.Succeeded || activityRes.Result == null || !activityRes.Result.IsSuccess)
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(
                    new ApplicationException("Can't find activity id details"), "Can't find activity id details");
            }

            if(!activityRes.Result.Result.Schedules.Any(s => s.Id == args.ScheduleId))
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(
                    new ApplicationException("Invalid activity and schedule selected"), "Invalid activity and schedule selected");
            }

            var createOngoingActivityRes = await ongoingActivitiesData.CreateOngoingActivity(
                new Framework.ApiCommand.ApiData.OngoingActivity.Request.CreateOngoingActivityArgs 
            {
                ActivityId = args.ActivityId,
                CustomerId = args.CustomerId,
                PurchaseOrderId = args.PurchaseOrderId,
                ScheduleId = args.ScheduleId
            });

            if(!createOngoingActivityRes.Succeeded || createOngoingActivityRes.Result == null)
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(
                    new ApplicationException(createOngoingActivityRes.Message), createOngoingActivityRes.Message);
            }

            if(createOngoingActivityRes.Succeeded && !createOngoingActivityRes.Result.IsSuccess)
            {
                return AppResult<CreateOngoingActivityResult>.CreateFailed(
                    new ApplicationException(createOngoingActivityRes.Result.ErrorInfo?.Message), "An error occured in CreateOngoingActivityHandler");
            }

            // enroll the students
            

            return AppResult<CreateOngoingActivityResult>.CreateSucceeded(new CreateOngoingActivityResult {
                CreatedId = createOngoingActivityRes.Result.Result.Id,
            }, "Successfully created ongoing activity");

        }
        catch (Exception ex)
        {
            return AppResult<CreateOngoingActivityResult>.CreateFailed(ex, "An error occured in CreateOngoingActivityHandler");
        }
    }
}