using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ChatService.Interactors;

public class GetRequestMessageArgs : IInteractor
{
    public string Guid {get; set;}
    public string Token {get; set;}
}