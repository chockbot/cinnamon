using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;
public class GetDirectStudentSalesHandler : IGetDirectStudentSalesHandler
{
    private readonly IDirectStudentData directStudentData;
    public GetDirectStudentSalesHandler(IDirectStudentData directStudentData)
    {
        this.directStudentData = directStudentData;
    }

    public AppResult<GetDirectStudentSalesResult> Execute(GetDirectStudentSalesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetDirectStudentSalesResult>.CreateFailed(ex, "An error occured in GetDirectStudentSalesHandler");
        }
    }

    public async Task<AppResult<GetDirectStudentSalesResult>> ExecuteAsync(GetDirectStudentSalesArgs args)
    {
        try
        {
            var result = await directStudentData.GetDirectStudentsPayments(new Framework.ApiCommand.ApiData.DirectStudent.Request.GetDirectStudentsPaymentArgs
            {
                ProviderId = args.Id,
                DateFrom   = args.DateFrom.ToString("yyyyMMddHHmmss")
            });
            if (!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return AppResult<GetDirectStudentSalesResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }
            return AppResult<GetDirectStudentSalesResult>.CreateSucceeded(new GetDirectStudentSalesResult
            {
                DirectStudentSales = result.Result.Result.Select(e =>
                {
                    return new GetDirectStudentSalesResult.DirectStudentSale
                    {
                        Amount = e.Amount,
                        PurchaseDate = e.CreatedOn
                    };
                })
            }, "Successfully direct students sales");

        }
        catch (Exception ex)
        {
            return AppResult<GetDirectStudentSalesResult>.CreateFailed(ex, "An error occurred in GetDirectStudentSalesHandler");
        }
    }
}
