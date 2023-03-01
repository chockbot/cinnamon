namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.RequestMessage;

public class RequestPaymentArgs 
{
    public string currency {get; set;}
    public decimal amount {get; set;}
    public string country {get; set;}
    public string reference_id {get; set;}
    public PaymentMethod payment_method {get; set;}

    public class PaymentMethod 
    {
        public string type {get; set;}
        public string reusability {get; set;}
        public string country {get; set;}
        public EWallet ewallet {get; set;}
    }

    public class EWallet 
    {
        public string channel_code {get; set;}
        public Channel_Properties channel_properties {get; set;}
    }

    public class Channel_Properties 
    {
        public string success_return_url {get; set;}
        public string failure_return_url {get; set;}
        public string cancel_return_url {get; set;}
    }
}