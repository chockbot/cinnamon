using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
using Flurl;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class TransactionRedirectionHandler : ITransactionRedirectionHandler
{
    private readonly ITokenGeneratedData tokenGeneratedData;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly ApplicationConfig applicationConfig;

    public TransactionRedirectionHandler(ITokenGeneratedData tokenGeneratedData, IJsonSerializationProvider jsonSerializationProvider,
        IPurchaseOrderData purchaseOrderData, IGetActivityHandler getActivityHandler, ApplicationConfig applicationConfig)
    {
        this.tokenGeneratedData = tokenGeneratedData;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.purchaseOrderData = purchaseOrderData;
        this.getActivityHandler = getActivityHandler;
        this.applicationConfig = applicationConfig;
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
            
            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = deserializedPayload.ActivityId,
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                throw new ApplicationException("Unable to get purchased activity.");
            }
            var activity = activityRes.Result;

            string redirect = string.Empty;

            if(transaction.Status == 0)
            {
                redirect = activity.ExperienceCreationType switch {
                    Framework.Enums.Enums.ExperienceCreationType.GeneralExperience => 
                        applicationConfig.FrontendUrl
                            .AppendPathSegment($"payment/{transaction.ActivityId}/{transaction.ScheduleId}")
                            .SetQueryParam("Status", "failed"),
                    Framework.Enums.Enums.ExperienceCreationType.OneTimeEvents =>
                        applicationConfig.FrontendUrl
                            .AppendPathSegment($"payment/ote/{activity.Handler}")
                            .SetQueryParam("Ticket", deserializedPayload.OteQuery)
                            .SetQueryParam("Status","failed"),
                    _ => "/not-found"
                };

                return AppResult<TransactionRedirectionResult>.CreateSucceeded(new TransactionRedirectionResult {
                    RedirectUrl = redirect
                }, "Successfully validate and redirect transaction.");
            }

            bool successTransaction = transaction.Status == 1 || transaction.Status == 5;
            if(!successTransaction)
            {
                redirect = activity.ExperienceCreationType switch {
                    Framework.Enums.Enums.ExperienceCreationType.GeneralExperience => 
                        applicationConfig.FrontendUrl
                            .AppendPathSegment($"payment/{transaction.ActivityId}/{transaction.ScheduleId}")
                            .SetQueryParam("Status", "failed"),
                    Framework.Enums.Enums.ExperienceCreationType.OneTimeEvents =>
                        applicationConfig.FrontendUrl
                            .AppendPathSegment($"payment/ote/{activity.Handler}")
                            .SetQueryParam("Ticket", deserializedPayload.OteQuery)
                            .SetQueryParam("Status","failed"),
                    _ => "/not-found"
                };

                return AppResult<TransactionRedirectionResult>.CreateSucceeded(new TransactionRedirectionResult {
                    RedirectUrl = redirect
                }, "Successfully validate and redirect transaction.");
            }

            redirect = activity.ExperienceCreationType switch {
                Framework.Enums.Enums.ExperienceCreationType.GeneralExperience => 
                    applicationConfig.FrontendUrl
                        .AppendPathSegment("purchase/order")
                        .SetQueryParam("purchaseid", transaction.Id),
                Framework.Enums.Enums.ExperienceCreationType.OneTimeEvents =>
                    applicationConfig.FrontendUrl
                        .AppendPathSegment("purchase/order/ote")
                        .AppendPathSegment(transaction.Id),
                _ => "/not-found"
            };

            redirect = string.IsNullOrEmpty(deserializedPayload.Url) ? 
                        applicationConfig.FrontendUrl.AppendPathSegment("not-found") : deserializedPayload.Url;

            return AppResult<TransactionRedirectionResult>.CreateSucceeded(new TransactionRedirectionResult {
                RedirectUrl = redirect,
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
        public int ScheduleId {get; set;}
        public int TransactionId {get; set;}
        public string OteQuery {get; set;}
        public string Url {get; set;}
    }
}