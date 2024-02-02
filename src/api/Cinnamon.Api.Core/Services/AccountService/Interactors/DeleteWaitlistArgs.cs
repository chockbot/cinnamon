using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class DeleteWaitlistArgs : IInteractor
{
    public string Email { get; set; }
}
