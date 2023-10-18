using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class OteTicketDetailsArgs : IInteractor
{
    public string Guid {get; set;}
    public string Token {get; set;}
}