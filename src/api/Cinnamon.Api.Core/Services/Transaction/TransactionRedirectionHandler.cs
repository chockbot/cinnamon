using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class TransactionRedirectionHandler : ITransactionRedirectionHandler
{
    private readonly ITokenGeneratedData tokenGeneratedData;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IPurchaseOrderData purchaseOrderData;

    public TransactionRedirectionHandler(ITokenGeneratedData tokenGeneratedData, IJsonSerializationProvider jsonSerializationProvider,
        IPurchaseOrderData purchaseOrderData)
    {
        this.tokenGeneratedData = tokenGeneratedData;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.purchaseOrderData = purchaseOrderData;
    }

    public AppResult<TransactionRedirectionResult> Execute(TransactionRedirectionArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<TransactionRedirectionResult>> ExecuteAsync(TransactionRedirectionArgs args)
    {
        try
        {
            if(string.IsNullOrEmpty(args.Guid) || string.IsNullOrEmpty(args.Token))
            {
                return AppResult<TransactionRedirectionResult>.CreateFailed(
                    new ApplicationException("Invalid request. Action not allowed."),
                    "Invalid request. Action not allowed."
                );
            }

            var tokenRes = await tokenGeneratedData.GetTokenGenerated(args.Guid, args.Token);
            if(!tokenRes.Succeeded || tokenRes.Result is null || !tokenRes.Result.IsSuccess)
            {
                return AppResult<TransactionRedirectionResult>.CreateFailed(
                    new ApplicationException(tokenRes.Result?.ErrorInfo?.Message),
                    tokenRes.Message
                );
            }
            var tokenData = tokenRes.Result.Result;

            bool validTokenForTransaction = tokenData.TokenType.Equals("TRANSACTION-REQUEST", StringComparison.CurrentCultureIgnoreCase);
            if(!validTokenForTransaction)
            {
                return AppResult<TransactionRedirectionResult>.CreateFailed(
                    new ApplicationException("Invalid request. Action not allowed."),
                    "Invalid request. Action not allowed."
                );
            }

            var deserializedPayload = jsonSerializationProvider.Deserialize<TokenPayload>(tokenData.Payload);
            if(deserializedPayload is null)
            {
                throw new ApplicationException("An error occured when trying to deserialized payload");
            }

            var transactionRes = await purchaseOrderData.GetPurchaseOrderById(deserializedPayload.TransactionId);
            if(!transactionRes.Succeeded || transactionRes.Result is null || !transactionRes.Result.IsSuccess)
            {
                throw new ApplicationException("Unable to get purchase order transaction");
            }
            var transaction = transactionRes.Result.Result;

            if(transaction.Status == 0)
            {
                return AppResult<TransactionRedirectionResult>.CreateSucceeded(new TransactionRedirectionResult {
                    RedirectUrl = deserializedPayload.FailedUrl
                }, "Successfully validate and redirect transaction.");
            }

            bool successTransaction = transaction.Status == 1 || transaction.Status == 5;
            if(!successTransaction)
            {
                return AppResult<TransactionRedirectionResult>.CreateSucceeded(new TransactionRedirectionResult {
                    RedirectUrl = deserializedPayload.FailedUrl
                }, "Successfully validate and redirect transaction.");
            }

            return AppResult<TransactionRedirectionResult>.CreateSucceeded(new TransactionRedirectionResult {
                RedirectUrl = deserializedPayload.SuccessUrl,
            }, "Successfully validate and redirect transcation.");
        }
        catch (Exception ex)
        {
            return AppResult<TransactionRedirectionResult>.CreateFailed(ex, "An error occured in TransactionRedirectionHandler.");
        }
    }

    private record TokenPayload 
    {
        public int ActivityId {get; set;}
        public int TransactionId {get; set;}
        public string SuccessUrl {get; set;}
        public string FailedUrl {get; set;}
    }
}