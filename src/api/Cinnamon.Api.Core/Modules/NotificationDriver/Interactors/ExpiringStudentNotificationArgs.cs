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
    public string ImageLocation { get; set; }
    public string APrice { get; set; }
    public string Address { get; set; }
    public decimal Rating { get; set; }
    public int Count { get; set; }
}