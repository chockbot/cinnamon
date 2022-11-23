using System;
using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.EmailService.Interactors;

public class SendMail : IInteractor 
{
    public IList<string> Recipients {get; set;}
    public string Subject {get; set;}
    public string Body {get; set;}
    // content type default to text
    public string ContentType { get; set; } = "text";
}