using Cinnamon.Framework.ApiCommand.ApiCore.Payment.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Payment.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface IPaymentApiHandler 
{
    Task<AppResult<VerifyCallbackResult>> VerifyCallback(VerifyCallbackArgs args);
    Task<AppResult<GetPaymentChannelsResult>> GetPaymentChannels();
    Task<AppResult<VerifyPayoutCallbackResult>> VerifyPayoutCallback(VerifyPayoutCallbackArgs args);
} 
