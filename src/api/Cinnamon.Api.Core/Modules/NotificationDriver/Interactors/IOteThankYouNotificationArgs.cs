using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;

public class IOteThankYouNotificationArgs : IInteractor 
{
    public string EventName {get; set;}
    public string Subject {get; set;}
    public string Body {get; set;}
    public string CustomerName {get; set;}
    public string CustomerEmail {get; set;}
    public DateTime EventDate {get; set;}
    public string EventLocation {get; set;}
    public int DaysStart {get; set;}
}