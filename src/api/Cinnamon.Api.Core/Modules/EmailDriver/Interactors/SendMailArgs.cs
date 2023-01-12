using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.EmailDriver.Interactors;

public class SendMailArgs : IInteractor
{
    public IEnumerable<string> Recipients {get; set;}
    public string Subject {get; set;}
    public string Body {get; set;}
    // content type default to text
    public string ContentType { get; set; } = "text";
}