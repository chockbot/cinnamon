using Cinnamon.Core.Enums;
using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.CinnamonMakerService.Interactors;

public class VerifyEmail : IInteractor
{
    public string UserId { get; set; }
    public string Token { get; set; }
}