using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;

public class MakerEnrolledNotificationArgs : IInteractor 
{
    public string ExperienceName {get; set;}
    public decimal Amount {get; set;}
    public DateTime PurchaseDate {get; set;}
    public string Email {get; set;}
    public string MakerName {get; set;}
    public string PayerName {get; set;}
    public IEnumerable<IncludedStudents> Students {get; set;}
    public string PaymentMethod {get; set;}
    public string ReferenceNumber {get; set;}

    public class IncludedStudents
    {
        public string Name {get; set;}
    }
}