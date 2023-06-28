using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;

public class CustomerPayedNotificationArgs : IInteractor 
{
    public string Email {get; set;}
    public string CustomerName {get; set;}
    public string ExperienceName {get; set;}
    public string CoachName {get; set;}
    public string CoachNumber { get; set; }
    public DateTime PurchaseDate {get; set;}
    public string PayerName {get; set;}
    public decimal Amount {get; set;}
    public decimal ServiceFee {get; set;}
    public IEnumerable<IncludedMembers> Members {get; set;}
    public string PaymentMethod {get; set;}
    public string ReferenceNumber {get; set;}
    public string MakerEmail {get; set;}
    public decimal PaymentProviderFee {get; set;}
    public decimal AppliedCredits {get; set;}
    public bool IsInclusivePayment {get; set;}
    public decimal DiscountAmount {get; set;}

    public class IncludedMembers 
    {
        public string Name {get; set;}
    }
}