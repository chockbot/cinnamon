using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.EmailDriver.Interactors;

public class SendInviteEventArgs : IInteractor
{
    public IEnumerable<Attendee> Attendees {get; set;}
    public string Subject {get; set;}
    public string Content {get; set;}
    public DateTime DateStart {get; set;}
    public DateTime DateEnd {get; set;}
    public string Location {get; set;}

    public class Attendee 
    {
        public string Name {get; set;}

        public string Email {get; set;}
    }
}