using Cinnamon.Framework.Interactor;
namespace Cinnamon.Api.Core.Services.AccountService.Interactors;

public class SendOTPArgs : IInteractor
{
    public string Email { get; set; }
    public int[] OTPCode { get; set; }
}
