using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteTicketBookedCountHandler : IOteTicketBookedCountHandler
{
    private readonly IOteTicketData oteTicketData;
    private readonly IGetOwnedActivityHandler getOwnedActivityHandler;
    private readonly IOteDateData oteDateData;
    private readonly IMapper mapper;

    public OteTicketBookedCountHandler(IOteTicketData oteTicketData, IGetOwnedActivityHandler getOwnedActivityHandler,
        IOteDateData oteDateData, IMapper mapper)
    {
        this.oteTicketData = oteTicketData;
        this.getOwnedActivityHandler = getOwnedActivityHandler;
        this.oteDateData = oteDateData;
        this.mapper = mapper;
    }
    
    public AppResult<OteTicketBookedCountResult> Execute(OteTicketBookedCountArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteTicketBookedCountResult>> ExecuteAsync(OteTicketBookedCountArgs args)
    {
        try
        {
            var activityRes = await getOwnedActivityHandler.ExecuteAsync(new GetOwnedActivityArgs {
                ActivityId = args.ActivityId,
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<OteTicketBookedCountResult>.CreateFailed(new ApplicationException("Action not allowed. Invalid request."), "Action not allowed. Invalid request.");
            }

            var bookedCountRes = await oteTicketData.CountBookedTickets(new Framework.ApiCommand.ApiData.OteTicket.Request.CountBookedTicketsArgs {
                ActivityId = args.ActivityId
            });
            if(!bookedCountRes.Succeeded || bookedCountRes.Result is null || !bookedCountRes.Result.IsSuccess)
            {
                return AppResult<OteTicketBookedCountResult>.CreateFailed(new ApplicationException(bookedCountRes.Message), bookedCountRes.Message);
            }
            var bookedCount = bookedCountRes.Result.Result;

            var firstOteDateSchedule = await oteDateData.GetFirst(new Framework.ApiCommand.ApiData.OteDate.Request.GetFirstArgs {
                ActivityId = args.ActivityId
            });
            if(!firstOteDateSchedule.Succeeded || firstOteDateSchedule.Result is null || !firstOteDateSchedule.Result.IsSuccess)
            {
                return AppResult<OteTicketBookedCountResult>.CreateFailed(
                    new ApplicationException(firstOteDateSchedule.Message), firstOteDateSchedule.Message);
            }
            var dateSchedule = mapper.Map<OteTicketBookedCountResult.FirstScheduleDate>(firstOteDateSchedule.Result.Result);

            return AppResult<OteTicketBookedCountResult>.CreateSucceeded(
                new OteTicketBookedCountResult {BookedCount = bookedCount, FirstOteDate = dateSchedule}, "Successfully get ticket booked count.");
        }
        catch (Exception ex)
        {
            return AppResult<OteTicketBookedCountResult>.CreateFailed(ex, "An error occured in OteTicketBookedCountHandler.");
        }
    }
}