using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;

public class ExpiringStudentNotificationArgs : IInteractor 
{
    public string Email {get; set;}
    public DateTime DateSend { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime ExpiredDate { get; set; }
    public string ActivityTitle { get; set; }
    public decimal Amount { get;  set;}
    public string ActivityLink { get; set; }
}