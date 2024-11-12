using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;
public class GetOTEByActivityIdHandler : IGetOTEByActivityIdHandler
{
    private readonly IOteTicketData oteTicketData;
    public GetOTEByActivityIdHandler(IOteTicketData oteTicketData)
    {
        this.oteTicketData = oteTicketData;
    }

    public AppResult<GetOTEByActivityIdResult> Execute(GetOTEByActivityIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetOTEByActivityIdResult>.CreateFailed(ex, "An error occurred in GetOTEByActivityIdHandler");
        }
    }

    public async Task<AppResult<GetOTEByActivityIdResult>> ExecuteAsync(GetOTEByActivityIdArgs args)
    {
        try
        {
            var result = await oteTicketData.GetByActivityId(args.ActivityId, new Framework.ApiCommand.ApiData.OteTicket.Request.GetByActivityIdArgs
            {
                SearchValue          = args.SearchValue ?? string.Empty,
                SearchBy             = args.SearchBy ?? 0,
                CountPerPage         = args.CountPerPage,
                PageIndex            = args.PageIndex,
                IncludeCustomer      = true,
                IncludeImageAsResult = false,
                DateId = args.DateId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetOTEByActivityIdResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetOTEByActivityIdResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetOTEByActivityIdHandler");
            }
            return AppResult<GetOTEByActivityIdResult>.CreateSucceeded(new GetOTEByActivityIdResult
            {
                OTEDetails = result.Result.Result.Select(e =>
                {
                    return new GetOTEByActivityIdResult.OTEDetail
                    {
                        Id     = e.Id,
                        Title  = e.Title,
                        Amount = e.Amount,
                        QRCode = e.QRCode,
                        Status = e.Status,
                        Customer = new Framework.ApiCommand.ApiData.DTO.Customer.CustomerDTO
                        {
                            FirstName = e.Customer.FirstName,
                            LastName  = e.Customer.LastName,
                            Email     = e.Customer.Email,   
                        },
                        PurchaseOrder = new Framework.ApiCommand.ApiData.DTO.PurchaseOrder.PurchaseOrderDTO
                        {
                            Payload = e.PurchaseOrder.Payload,
                        }
                    };
                }),
                ErrorInfo = new Framework.ApiCommand.ApiCore.ErrorInfo
                {
                    Code        = result?.Result?.ErrorInfo?.Code,
                    Description = result?.Result?.ErrorInfo?.Description,
                    Message     = result?.Result?.ErrorInfo?.Message
                },
                Pagination = new Framework.ApiCommand.ApiCore.Pagination
                {
                    PageIndex    = result.Result.Pagination.PageIndex,
                    PerPage      = result.Result.Pagination.PerPage,
                    TotalPages   = result.Result.Pagination.TotalPages,
                    TotalRecords = result.Result.Pagination.TotalRecords
                }
            }, "Successfully get customer list");
        }
        catch (Exception ex)
        {
            return AppResult<GetOTEByActivityIdResult>.CreateFailed(ex, "An error occured in GetOTEByActivityIdHandler");
        }
    }
}
