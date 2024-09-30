using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class GetAllTicketPurchasedHandler : IGetAllTicketPurchasedHandler
{
    private readonly IOteTicketData oteTicketData;
    public GetAllTicketPurchasedHandler(IOteTicketData oteTicketData)
    {
        this.oteTicketData = oteTicketData;
    }

    public AppResult<GetAllTicketPurchasedResult> Execute(GetAllTicketPurchasedArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetAllTicketPurchasedResult>.CreateFailed(ex, "An error occurred in GetAllTicketPurchasedHandler");
        }
    }

    public async Task<AppResult<GetAllTicketPurchasedResult>> ExecuteAsync(GetAllTicketPurchasedArgs args)
    {
        try
        {
            var result = await oteTicketData.GetAllTicketPurchased(new Framework.ApiCommand.ApiData.OteTicket.Request.GetAllTicketPurchasedArgs
            {
                ActivityId = args.ActivityId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetAllTicketPurchasedResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetAllTicketPurchasedResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetAllTicketPurchasedHandler");
            }
            return AppResult<GetAllTicketPurchasedResult>.CreateSucceeded(new GetAllTicketPurchasedResult
            {
                OTEDetails = result.Result.Result.Select(e =>
                {
                    return new GetAllTicketPurchasedResult.OTEDetail
                    {
                        Id       = e.Id,
                        Title    = e.Title,
                        Amount   = e.Amount,
                        QRCode   = e.QRCode,
                        Status   = e.Status,
                        Payload  = e.Payload,
                        Date     = e.Date,
                        Quantity = e.Quantity,
                        Customer = new Framework.ApiCommand.ApiData.DTO.Customer.CustomerDTO
                        {
                            FirstName = e.Customer.FirstName,
                            LastName  = e.Customer.LastName,
                            Email     = e.Customer.Email,
                        },
                    };
                }),
            }, "Successfully get customer list");
        }
        catch (Exception ex)
        {
            return AppResult<GetAllTicketPurchasedResult>.CreateFailed(ex, "An error occured in GetOTEByActivityIdHandler");
        }
    }
}
