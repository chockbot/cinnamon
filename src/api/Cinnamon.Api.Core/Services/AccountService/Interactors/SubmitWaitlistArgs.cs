using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class SubmitWaitlistArgs : IInteractor
{
    public string Email {get; set;}
    public string ValidationRoute {get; set;}
    public bool IsGuest { get; set; } = false;
}