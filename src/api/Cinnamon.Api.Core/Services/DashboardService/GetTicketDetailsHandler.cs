using Cinnamon.Api.Core.Modules.DataAccess.Activity;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;
public class GetTicketDetailsHandler : IGetTicketDetailsHandler
{
    private readonly IOteTicketData oteTicketData;
    public GetTicketDetailsHandler(IOteTicketData oteTicketData)
    {
        this.oteTicketData = oteTicketData;
    }

    public AppResult<GetTicketDetailsResult> Execute(GetTicketDetailsArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetTicketDetailsResult>.CreateFailed(ex, "An error occured in GetTicketDetailsHandler");
        }
    }

    public async Task<AppResult<GetTicketDetailsResult>> ExecuteAsync(GetTicketDetailsArgs args)
    {
        try
        {
            var result = await oteTicketData.GetTicketDetails(new Framework.ApiCommand.ApiData.OteTicket.Request.GetTicketDetailsArgs
            {
                ActivityId = args.ActivityId,
                DateId = args.DateId
            });
            if (!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return AppResult<GetTicketDetailsResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }

            return AppResult<GetTicketDetailsResult>.CreateSucceeded(new GetTicketDetailsResult
            {
                OTETickets = result.Result.Result.Select(s =>
                {
                    return new GetTicketDetailsResult.OTETicket
                    {
                        ActivityId  = s.ActivityId,
                        From        = s.From,
                        To          = s.To,
                        Recurrences = s.Recurrences,
                        OtePricingDTO = new Framework.ApiCommand.ApiCore.DTO.Activity.OtePricingDTO
                        {
                            Id                    = s.OteSchedulePricingDTO.Id,
                            Name                  = s.OteSchedulePricingDTO.Name,
                            Description           = s.OteSchedulePricingDTO.Description,
                            Sold                  = s.OteSchedulePricingDTO.Sold,
                            MaxSlots              = s.OteSchedulePricingDTO.MaxSlots,
                            Available             = s.OteSchedulePricingDTO.Available,
                            Price                 = s.OteSchedulePricingDTO.Price,
                            OteSchedulePricingsId = s.OteSchedulePricingDTO.OteSchedulePricingsId,
                            RequiredApproval      = s.OteSchedulePricingDTO.RequiredApproval,
                            IsUnlimited           = s.OteSchedulePricingDTO.IsUnlimited
                        }
                    };
                })
            }, "Successfully get ote activities");
        }
        catch (Exception ex)
        {
            return AppResult<GetTicketDetailsResult>.CreateFailed(ex, "An error occured in GetTicketDetailsHandler");
        }
    }
}
