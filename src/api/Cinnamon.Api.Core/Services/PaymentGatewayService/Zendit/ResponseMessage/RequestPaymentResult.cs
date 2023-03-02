namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.ReponseMessage;

public class RequestPaymentResult 
{
    public IEnumerable<ProceedAction> Actions {get; set;}
    public decimal Amount {get; set;}
    public string Capture_Method {get; set;}
    public string Status {get; set;}
    public string Customer_Id {get; set;}
    public string Reference_Id  {get; set;}

    public class ProceedAction
    {
        public string Action {get; set;}
        public string Method {get; set;}
        public string Url {get; set;}
        public string Url_Type {get; set;}
    }
}