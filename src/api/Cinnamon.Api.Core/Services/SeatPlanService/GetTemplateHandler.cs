using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.SeatPlanService.Handler;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors.Result;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.SeatPlanService;

public class GetTemplateHandler : IGetTemplateHandler
{
    private readonly ISeatPlanData seatPlanData;
    private readonly IMapper mapper;

    public GetTemplateHandler(ISeatPlanData seatPlanData, IMapper mapper)
    {
        this.mapper = mapper;
        this.seatPlanData = seatPlanData;
    }

    public AppResult<GetTemplateResult> Execute(GetTemplateArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<GetTemplateResult>> ExecuteAsync(GetTemplateArgs args)
    {
        try
        {
            var templateRes = await seatPlanData.GetSeatPlanTemplateByIdAsync(args.Id);
            if(!templateRes.Succeeded || templateRes.Result is null || !templateRes.Result.IsSuccess)
            {
                return AppResult<GetTemplateResult>.CreateFailed(new ApplicationException(templateRes.Result?.ErrorInfo?.Message), templateRes.Message);
            }

            var template = mapper.Map<GetTemplateResult>(templateRes.Result.Result);
            return AppResult<GetTemplateResult>.CreateSucceeded(template, "Template retrieved successfully");
        }
        catch (System.Exception ex)
        {
            return AppResult<GetTemplateResult>.CreateFailed(ex, "An error occuredi in GetTemplateHandler");
        }
    }
}