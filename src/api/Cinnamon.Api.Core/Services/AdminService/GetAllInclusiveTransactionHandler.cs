using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AdminService;

public class GetAllInclusiveTransactionHandler : IGetAllInclusiveTransactionHandler
{
    private readonly IPurchaseOrderData purchaseOrderData;

    public GetAllInclusiveTransactionHandler(IPurchaseOrderData purchaseOrderData)
    {
        this.purchaseOrderData = purchaseOrderData;
    }

    public AppResult<GetAllInclusiveTransactionResult> Execute(GetAllInclusiveTransactionArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetAllInclusiveTransactionResult>.CreateFailed(ex, "An error occured in GetAllInclusiveTransactionHandler");
        }
    }

    public async Task<AppResult<GetAllInclusiveTransactionResult>> ExecuteAsync(GetAllInclusiveTransactionArgs args)
    {
        try
        {
            var result = await purchaseOrderData.GetAllInclusiveTransactions(new Framework.ApiCommand.ApiData.PurchaseOrder.Request.GetAllInclusiveTransactionArgs {
                Email  = args.Email,
                Name = args.Name,
                PurchaseDateFrom = args.PurchaseDateFrom,
                PurchaseDateTo = args.PurchaseDateTo,
                Status = args.Status
            });
            if(!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return AppResult<GetAllInclusiveTransactionResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }

            return AppResult<GetAllInclusiveTransactionResult>.CreateSucceeded(new GetAllInclusiveTransactionResult {
                Transactions = result.Result.Result.Select(t => new GetAllInclusiveTransactionResult.Transaction {
                    ConvinienceFee = t.ConvinienceFee,
                    CreditAmount = t.CreditAmount,
                    OverallTotal = t.OverallTotal,
                    PurchaseDate = t.PurchaseDate,
                    PurchaseOrderId = t.PurchaseOrderId,
                    Status = t.Status,
                    Total = t.Total,
                    UnitCount = t.UnitCount,
                    UnitPrice = t.UnitPrice,
                    Provider = new GetAllInclusiveTransactionResult.Transaction.ProviderDTO {
                        Email = t.Provider.Email,
                        FirstName = t.Provider.FirstName,
                        LastName = t.Provider.LastName
                    }
                })
            }, "Successfully get all inclusive transactions");
        }
        catch (Exception ex)
        {
            return AppResult<GetAllInclusiveTransactionResult>.CreateFailed(ex, "An error occured in GetAllInclusiveTransactionHandler");
        }
    }
}