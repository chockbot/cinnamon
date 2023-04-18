namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Zendit.ReponseMessage;

public class ErrorResponse 
{
    public string error_code {get; set;}
    public string message {get; set;}
    public IEnumerable<ErrorObj> errors {get; set;}

    public class ErrorObj 
    {
        public string message {get; set;}
    }
}