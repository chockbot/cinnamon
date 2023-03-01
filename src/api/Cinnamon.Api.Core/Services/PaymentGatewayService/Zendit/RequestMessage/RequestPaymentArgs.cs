namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.RequestMessage;

public class RequestPaymentArgs 
{
    public string Currency {get; set;}
    public decimal Amount {get; set;}
    public string Country {get; set;}
    public string Reference_Id {get; set;}
    public string Customer_Id {get; set;}
    public PaymentMethod Payment_Method {get; set;}

    public class PaymentMethod 
    {
        public string Type {get; set;}
        public string Reusability {get; set;}
        public string Country {get; set;}
        public EWallet EWallet {get; set;}
    }

    public class EWallet 
    {
        public string Channel_Code {get; set;}
        public Channel_Properties Channel_Properties {get; set;}
    }

    public class Channel_Properties 
    {
        public string Success_Return_Url {get; set;}
        public string Failure_Return_Url {get; set;}
        public string Cancel_Return_Url {get; set;}
    }
}