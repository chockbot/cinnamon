namespace Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;

public class OtePurchaseOrderResult 
{
    public int Id {get; set;}
    // 0 = no action, 1 = redirect
    public int Action {get; set;}
    public string Url {get; set;}
}