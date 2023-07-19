using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class VerifyUserNotificationArgs : IInteractor
{
    public string Email { get; set; }
    public string IdAttached { get; set; }
    public string BankDetails { get; set; }
}