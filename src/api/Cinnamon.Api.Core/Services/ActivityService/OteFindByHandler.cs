using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteFindByHandler : IOteFindByHandler
{
    private readonly IActivityData activityData;
    private readonly IMapper mapper;

    public OteFindByHandler(IActivityData activityData, IMapper mapper)
    {
        this.activityData = activityData;
        this.mapper = mapper;
    }

    public AppResult<OteFindByHandlerResult> Execute(OteFindByHandlerArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {   
            return AppResult<OteFindByHandlerResult>.CreateFailed(ex, "An error occured in IOteFindByHandler");
        }
    }

    public async Task<AppResult<OteFindByHandlerResult>> ExecuteAsync(OteFindByHandlerArgs args)
    {
        try
        {
            var oteRes = await activityData.GetOteActivityByHandler(new Framework.ApiCommand.ApiData.Activity.Request.GetOteActivityArgs {
                IncludeAddress = args.IncludeAddress,
                IncludeDescription = args.IncludeDescription,
                IncludePricing = args.IncludePricing,
                IncludeSchedule = args.IncludeSchedule,
                IncludeImages = args.IncludeImages
            }, args.Handler);
            if(!oteRes.Succeeded || oteRes.Result is null || !oteRes.Result.IsSuccess)
            {
                return AppResult<OteFindByHandlerResult>.CreateFailed(new ApplicationException(oteRes.Message), oteRes.Message);
            }

            var result = mapper.Map<OteActivityDTO, OteFindByHandlerResult>(oteRes.Result.Result);
            return AppResult<OteFindByHandlerResult>.CreateSucceeded(result, "One time event successfully find by handler");
        }
        catch (Exception ex)
        {
            return AppResult<OteFindByHandlerResult>.CreateFailed(ex, "An error occured in IOteFindByHandler");
        }
    }
}