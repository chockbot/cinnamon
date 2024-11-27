using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;
public class SendEmailOTPArgs : IInteractor
{
    public string Email { get; set; }
    public int[] OTPCode { get; set; }
}
