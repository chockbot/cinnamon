using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class GetGrossSalesByProviderHandler : IGetGrossSalesByProviderHandler
{
	private readonly IPurchaseOrderData purchaseOrderData;

	public GetGrossSalesByProviderHandler(IPurchaseOrderData purchaseOrderData)
	{
		this.purchaseOrderData = purchaseOrderData;
	}

	public AppResult<GetGrossSalesByProviderResult> Execute(GetGrossSalesByProviderArgs args)
	{
		try
		{
			return ExecuteAsync(args).Result;
		}
		catch (Exception ex)
		{
			return AppResult<GetGrossSalesByProviderResult>.CreateFailed(ex, "An error occured in GetGrossSalesByProviderHandler");
		}
	}

	public async Task<AppResult<GetGrossSalesByProviderResult>> ExecuteAsync(GetGrossSalesByProviderArgs args)
	{
		try
		{
			var result = await purchaseOrderData.GetGrossSalesByProvider(new Framework.ApiCommand.ApiData.PurchaseOrder.Request.GetGrossSalesByProviderArgs
			{
				Id       = args.Id,
				DateFrom = args.DateFrom.ToString("yyyyMMddHHmmss")
			});
			if (!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
			{
				return AppResult<GetGrossSalesByProviderResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
			}
			return AppResult<GetGrossSalesByProviderResult>.CreateSucceeded(new GetGrossSalesByProviderResult
			{
				GrossSales = result.Result.Result.Select(e =>
				{
					return new GetGrossSalesByProviderResult.GrossSale
					{
						Id           = e.Id,
						ActivityId   = e.ActivityId,
						CustomerId   = e.CustomerId,
						Payload      = e.Payload,
						PurchaseDate = e.PurchaseDate,
						ScheduleId   = e.ScheduleId,
						Status       = e.Status,
						Total        = e.Total,
						UnitCount    = e.UnitCount,
						UnitPrice    = e.UnitPrice
					};
				})
			}, "Successfully get gross sales");

		}
		catch (Exception ex)
		{
			return AppResult<GetGrossSalesByProviderResult>.CreateFailed(ex, "An error occurred in GetGrossSalesByProviderHandler");
		}
	}
}
