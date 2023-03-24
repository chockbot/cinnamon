using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GetRequestRefundHandler : IGetRequestRefundHandler
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IRequestRefundData requestRefundData;

    public GetRequestRefundHandler(IHttpContextAccessor httpContext, IRequestRefundData requestRefundData)
    {
        this.httpContext = httpContext;
        this.requestRefundData = requestRefundData;
    }
    
    public AppResult<GetRequestRefundResult> Execute(GetRequestRefundArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetRequestRefundResult>.CreateFailed(ex, "An error occured in GetRequestRefundHandler");
        }
    }

    public async Task<AppResult<GetRequestRefundResult>> ExecuteAsync(GetRequestRefundArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<GetRequestRefundResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            var refundArgs = args.IsAdmin.GetValueOrDefault() ? new Framework.ApiCommand.ApiData.RequestRefund.Request.GetAllRequestRefundArgs
            {
                Status = args.Status,
                CountPerPage = args.CountPerPage,
                PageIndex = args.PageIndex,
                IncludeCustomer = args.IncludeCustomer,
                IncludePurchaseOrder = args.IncludePurchaseOrder
            } : new Framework.ApiCommand.ApiData.RequestRefund.Request.GetAllRequestRefundArgs
            {
                CustomerId = id,
                Status = args.Status,
                CountPerPage = args.CountPerPage,
                PageIndex = args.PageIndex,
                IncludeCustomer = args.IncludeCustomer,
                IncludePurchaseOrder = args.IncludePurchaseOrder
            };

            var result = await requestRefundData.GetAllRequestRefund(refundArgs);

            if(!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return AppResult<GetRequestRefundResult>.CreateFailed(
                    new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }

            return AppResult<GetRequestRefundResult>.CreateSucceeded(new GetRequestRefundResult {
                RequestedRefunds = result.Result.Result.Select(r => {
                    return new GetRequestRefundResult.RequestedRefund {
                        Id              = r.Id,
                        ExperienceTitle = r.ExperienceTitle,
                        ReferenceNumber = "0000000000".Substring(r.Id.ToString().Length) + r.Id,
                        Status          = r.Status,
                        Email           = r.Customer?.Email,
                        FirstName       = r.Customer?.FirstName,
                        LastName        = r.Customer?.LastName,
                        Reason          = r.Reason,
                        OverAllTotal    = r.PurchaseOrder?.OverAllTotal
                    };
                }),
                Pagination = new Framework.ApiCommand.ApiCore.Pagination
                {
                    PageIndex = result.Result.Pagination.PageIndex,
                    PerPage = result.Result.Pagination.PerPage,
                    TotalPages = result.Result.Pagination.TotalPages,
                    TotalRecords = result.Result.Pagination.TotalRecords
                }
            }, "Successfully get requested refunds");
        }
        catch (Exception ex)
        {
            return AppResult<GetRequestRefundResult>.CreateFailed(ex, "An error occured in GetRequestRefundHandler");
        }
    }
}