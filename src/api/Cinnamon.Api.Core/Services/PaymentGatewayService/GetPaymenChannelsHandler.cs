using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.PaymentGatewayService;

public class GetPaymentChannelsHander : IGetPaymentChannelsHandler
{
    private readonly ApplicationConfig applicationConfig;

    public GetPaymentChannelsHander(ApplicationConfig applicationConfig)
    {
        this.applicationConfig = applicationConfig;
    }

    public AppResult<GetPaymentChannelResult> Execute(GetPaymentChannelsArgs args)
    {
        try
        {
            var channels = applicationConfig.Payment.Accounts.First().PaymentChannels;
            if(channels.Count() == 0)
            {
                return AppResult<GetPaymentChannelResult>.CreateFailed(new ApplicationException("No payment channels set"), "No payment channels set");
            }

            return AppResult<GetPaymentChannelResult>.CreateSucceeded(new GetPaymentChannelResult {
                PaymentChannels = channels.Select(c => {
                    return new GetPaymentChannelResult.PaymentChannel {
                        Code = c.Code,
                        Name = c.Name
                    };
                })
            }, "Successfully get payment channels");
        }
        catch (Exception ex)
        {
            return AppResult<GetPaymentChannelResult>.CreateFailed(ex, "An error occured in GetPaymentChannelsHander");
        }
    }

    public Task<AppResult<GetPaymentChannelResult>> ExecuteAsync(GetPaymentChannelsArgs args)
    {
        return Task.Run(() => Execute(args));
    }
}