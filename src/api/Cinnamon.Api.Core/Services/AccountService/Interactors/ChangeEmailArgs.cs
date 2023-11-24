using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;
public class ChangeEmailArgs : IInteractor
{
    public string CurrentEmail { get; set; }
    public string NewEmail { get; set; }
}

