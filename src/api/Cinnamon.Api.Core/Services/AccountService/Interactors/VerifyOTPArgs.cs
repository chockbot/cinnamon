using Cinnamon.Framework.Interactor;
namespace Cinnamon.Api.Core.Services.AccountService.Interactors;
public class VerifyOTPArgs : IInteractor
{
    public string Email { get; set; }
}
