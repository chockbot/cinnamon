using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.CinnamonMakerService.Interactors;

public class ResendEmailArgs : IInteractor 
{
    public string Email { get; set; }
}