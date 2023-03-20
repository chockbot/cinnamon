namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.RequestMessage;

public class PayoutArgs 
{
    public string reference_id {get; set;}
    public string channel_code {get; set;}
    public ChannelProperties channel_properties {get; set;}
    public decimal amount {get; set;}
    public string currency {get; set;}

    public class ChannelProperties 
    {
        public string account_holder_name {get; set;}
        public string account_number {get; set;}
    }
}