using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;

public class MakerEnrolledNotificationArgs : IInteractor 
{
    public string ExperienceName {get; set;}
    public decimal Amount {get; set;}
    public DateTime PurchaseDate {get; set;}
    public string Email {get; set;}
}