using System;
using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.NotificationService.Interactors;

public class EmailVerification : IInteractor 
{
    public string Email { get; set; }
    public string VerificationLink  { get; set; }
}