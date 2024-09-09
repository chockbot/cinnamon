using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.SeatPlanService.Handler;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors.Result;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Models.SeatPlan;

namespace Cinnamon.Api.Core.Services.SeatPlanService;

public class UpdateSeatStatusHandler : IUpdateSeatStatusHandler
{
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IActivityData activityData;

    public UpdateSeatStatusHandler(IGetActivityHandler getActivityHandler, IOteFindByHandler oteFindByHandler,
        IJsonSerializationProvider jsonSerializationProvider, IActivityData activityData)
    {
        this.getActivityHandler = getActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.activityData = activityData;
    }

    public AppResult<UpdateSeatStatusResult> Execute(UpdateSeatStatusArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<UpdateSeatStatusResult>> ExecuteAsync(UpdateSeatStatusArgs args)
    {
        try
        {
            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = args.ActivityId
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<UpdateSeatStatusResult>.CreateFailed(
                    new ApplicationException(activityRes.Message), activityRes.Message);
            }
            var activity = activityRes.Result;

            var oteRes = await oteFindByHandler.ExecuteAsync(new ActivityService.Interactors.OteFindByHandlerArgs {
                Handler = activity.Handler,
                IncludeSchedule = true,
                IncludePricing = true
            });
            if(!oteRes.Succeeded || oteRes.Result is null)
            {
                return AppResult<UpdateSeatStatusResult>.CreateFailed(
                    new ApplicationException(oteRes.Message), oteRes.Message);
            }
            var oteActivity = oteRes.Result;

            var oteDate = oteActivity.OteDates.FirstOrDefault(d => d.Date.Date == args.EventDate.Date);
            if(oteDate is null)
            {
                return AppResult<UpdateSeatStatusResult>.CreateFailed(
                    new ApplicationException("No OTE date found for the given date"), "No OTE date found for the given date");
            }

            var seatplanPayload = oteDate.SeatPlanPayload ?? string.Empty;

            var deserializedPayload = jsonSerializationProvider.Deserialize<Format>(seatplanPayload);
            if(deserializedPayload is null)
            {
                return AppResult<UpdateSeatStatusResult>.CreateFailed(
                    new ApplicationException("Failed to deserialize seat plan payload"), "Failed to deserialize seat plan payload");
            }

            if(deserializedPayload.Categories[args.CategoryId] is null)
            {
                return AppResult<UpdateSeatStatusResult>.CreateFailed(
                    new ApplicationException("Category not found in seat plan payload"), "Category not found in seat plan payload");
            }

            if(deserializedPayload.Categories[args.CategoryId].Rows[args.RowId] is null)
            {
                return AppResult<UpdateSeatStatusResult>.CreateFailed(
                    new ApplicationException("Row not found in seat plan payload"), "Row not found in seat plan payload");
            }

            if(deserializedPayload.Categories[args.CategoryId].Rows[args.RowId].Seats[args.SeatId] is null)
            {
                return AppResult<UpdateSeatStatusResult>.CreateFailed(
                    new ApplicationException("Seat not found in seat plan payload"), "Seat not found in seat plan payload");
            }

            var seat = deserializedPayload.Categories[args.CategoryId].Rows[args.RowId].Seats[args.SeatId];
            seat.Occupied = args.Occupied;

            var serializedPayload = jsonSerializationProvider.Serialize(seat);

            var updateOteDatePayload = await activityData.UpdateOteDatePayload(new Framework.ApiCommand.ApiData.Activity.Request.UpdateOteDatePayloadArgs {
                Id = oteDate.Id,
                Payload = serializedPayload
            });
            if(!updateOteDatePayload.Succeeded || updateOteDatePayload.Result is null || !updateOteDatePayload.Result.IsSuccess)
            {
                return AppResult<UpdateSeatStatusResult>.CreateFailed(
                    new ApplicationException(updateOteDatePayload.Result?.ErrorInfo?.Message), updateOteDatePayload.Message);
            }

            return AppResult<UpdateSeatStatusResult>.CreateSucceeded(new UpdateSeatStatusResult {
                IsSuccess = updateOteDatePayload.Result.IsSuccess
            }, "Seat status updated successfully");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateSeatStatusResult>.CreateFailed(ex, "An error occured in UpdateSeatStatusHandler");
        }
    }
}