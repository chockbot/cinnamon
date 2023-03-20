using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors.Results;

public class GetPaymentChannelResult 
{
    public IEnumerable<PaymentChannel> PaymentChannels {get; set;}

    public class PaymentChannel 
    {
        public string Name {get; set;}
        public string Code {get; set;}
    }
}