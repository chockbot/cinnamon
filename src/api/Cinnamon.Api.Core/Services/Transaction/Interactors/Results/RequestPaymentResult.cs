namespace Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;

public class RequestPaymentResult 
{
    // 0 = no action, 1 = redirect
    public int Action {get; set;}
    public string Url {get; set;}
}