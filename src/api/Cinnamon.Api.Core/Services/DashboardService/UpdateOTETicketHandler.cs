using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.DataAccess.Student;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;
public class UpdateOTETicketHandler : IUpdateOTETicketHandler
{
    private readonly IOteTicketData oteTicketData;
    public UpdateOTETicketHandler(IOteTicketData oteTicketData)
    {
        this.oteTicketData = oteTicketData; 
    }

    public AppResult<UpdateOTETicketResult> Execute(UpdateOTETicketArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateOTETicketResult>.CreateFailed(ex, "An error occurred in UpdateOTETicketHandler");
        }
    }

    public async Task<AppResult<UpdateOTETicketResult>> ExecuteAsync(UpdateOTETicketArgs args)
    {
        try
        {
            var updated = await oteTicketData.UpdateTicket(new Framework.ApiCommand.ApiData.OteTicket.Request.UpdateTicketArgs
            {
                Id = args.Id,
                Status = args.Status
            });
            if (!updated.Succeeded || updated.Result == null || !updated.Result.IsSuccess)
            {
                return AppResult<UpdateOTETicketResult>.CreateFailed(new ApplicationException(updated.Result?.ErrorInfo?.Message), updated.Message);
            }
            if (updated.Succeeded && !updated.Result.IsSuccess)
            {
                return AppResult<UpdateOTETicketResult>.CreateFailed(
                    new ApplicationException(updated.Result.ErrorInfo?.Message), "An error occurred in UpdateOTETicketHandler");
            }
            return AppResult<UpdateOTETicketResult>.CreateSucceeded(new UpdateOTETicketResult
            {
                Id         = updated.Result.Result.Id,
                Status     = updated.Result.Result.Status,
                ActivityId = updated.Result.Result.ActivityId,
                Amount     = updated.Result.Result.Amount,
                QRCode     = updated.Result.Result.QRCode,
                Title      = updated.Result.Result.Title,
            }, "Successfully update ote ticket status");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateOTETicketResult>.CreateFailed(ex, "An error occurred in UpdateOTETicketHandler");
        }
    }
}
