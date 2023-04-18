using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class CreateUpdatePayoutAccountArgs : IInteractor
{
    public string AccountHolder {get; set;}
    public string AccountNumber {get; set;}
    public string Payload {get; set;}
    public string BankChannel {get; set;}
}