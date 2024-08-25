using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.SeatPlanService.Handler;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors.Result;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.SeatPlanService;

public class GetTemplatesHandler : IGetTemplatesHandler
{
    private readonly ISeatPlanData seatPlanData;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGetAdminUserByEmailHandler getAdminUserByEmailHandler;
    private readonly IMapper mapper;

    public GetTemplatesHandler(ISeatPlanData seatPlanData, IGetProfileHandler getProfileHandler, 
        IGetAdminUserByEmailHandler getAdminUserByEmailHandler, IMapper mapper)
    {
        this.seatPlanData = seatPlanData;
        this.getProfileHandler = getProfileHandler;
        this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
        this.mapper = mapper;
    }

    public AppResult<GetTemplatesResult> Execute(GetTemplatesArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<GetTemplatesResult>> ExecuteAsync(GetTemplatesArgs args)
    {
        try
        {
            var currentLogin = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!currentLogin.Succeeded || currentLogin.Result is null)
            {
                return AppResult<GetTemplatesResult>.CreateFailed(new ApplicationException(currentLogin.Message), currentLogin.Message);
            }
            var profile = currentLogin.Result;

            var adminResult = await getAdminUserByEmailHandler.ExecuteAsync(new AdminService.Interactors.GetAdminUserByEmailArgs { Email = profile.Email });
            if(!adminResult.Succeeded || adminResult.Result is null)
            {
                return AppResult<GetTemplatesResult>.CreateFailed(
                    new ApplicationException("Account is not admin. Invalid request."), "Account is not admin. Invalid request.");
            }

            var templatesRes = await seatPlanData.GetSeatPlanTemplateAsync(new Framework.ApiCommand.ApiData.SeatPlan.Request.GetSeatPlanTemplateArgs {
                CountPerPage = args.Limit,
                PageIndex = args.Page,
                TemplateName = args.Name
            });
            if(!templatesRes.Succeeded || templatesRes.Result is null || !templatesRes.Result.IsSuccess)
            {
                return AppResult<GetTemplatesResult>.CreateFailed(
                    new ApplicationException(templatesRes.Result?.ErrorInfo?.Message), templatesRes.Message);
            }

            var templates = mapper.Map<List<GetTemplatesResult.Template>>(templatesRes.Result.Result);
            
            return AppResult<GetTemplatesResult>.CreateSucceeded(new GetTemplatesResult { Templates = templates }, "Successfully retrieved templates.");
        }
        catch (Exception ex)
        {
            return AppResult<GetTemplatesResult>.CreateFailed(ex, "An error occured in GetTemplatesHandler.");
        }
    }
}