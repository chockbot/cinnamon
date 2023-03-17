namespace Cinnamon.Api.Core.Services.AccountService.Interactors.Results;

public class CreateUpdatePayoutAccountResult 
{
    public int Id {get; set;}
    public string AccountHolder {get; set;}
    public string AccountNumber {get; set;}
    public string Payload {get; set;}
}