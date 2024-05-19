using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.ApiCommand.ApiData.OteDate.Request;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteScheduleDatesHandler : IOteScheduleDatesHandler
{
    private readonly IOteDateData oteDateData;
    private readonly IGetOwnedActivityHandler getOwnedActivityHandler;
    private readonly IMapper mapper;

    public OteScheduleDatesHandler(IOteDateData oteDateData, IGetOwnedActivityHandler getOwnedActivityHandler,
        IMapper mapper)
    {
        this.oteDateData = oteDateData;
        this.getOwnedActivityHandler = getOwnedActivityHandler;
        this.mapper = mapper;
    }

    public AppResult<OteScheduleDatesResult> Execute(OteScheduleDatesArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteScheduleDatesResult>> ExecuteAsync(OteScheduleDatesArgs args)
    {
        try
        {
            var ownedActivityRes = await getOwnedActivityHandler.ExecuteAsync(new GetOwnedActivityArgs {
                ActivityId = args.ActivityId,
            });
            if(!ownedActivityRes.Succeeded || ownedActivityRes.Result is null)
            {
                return AppResult<OteScheduleDatesResult>.CreateFailed(new ApplicationException("Action not allowed. Invalid request."), "Action not allowed. Invalid request.");
            }

            var oteScheduleDatesRes = await oteDateData.GetOteDate(new GetOteDateArgs {
                ActivityId = args.ActivityId,
                From = args.DateFrom?.ToString("yyyyMMdd"),
                To = args.DateTo?.ToString("yyyyMMdd")
            });
            if(!oteScheduleDatesRes.Succeeded || oteScheduleDatesRes.Result is null || !oteScheduleDatesRes.Result.IsSuccess)
            {
                return AppResult<OteScheduleDatesResult>.CreateFailed(
                    new ApplicationException(oteScheduleDatesRes.Result?.ErrorInfo?.Message), oteScheduleDatesRes.Message);
            }

            var result = mapper.Map<IEnumerable<OteScheduleDatesResult.OteDateSchedule>>(oteScheduleDatesRes.Result.Result);
            return AppResult<OteScheduleDatesResult>.CreateSucceeded(
                new OteScheduleDatesResult {OteDateSchedules = result}, "Successfully get ote schedule dates.");
        }
        catch (Exception ex)
        {
            return AppResult<OteScheduleDatesResult>.CreateFailed(ex, "An error occured in OteScheduleDatesHandler.");
        }
    }
}