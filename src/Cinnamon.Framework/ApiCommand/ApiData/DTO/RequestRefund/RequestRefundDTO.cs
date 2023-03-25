namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.RequestRefund;

public class RequestRefundDTO 
{
    public int Id {get; set;}
    public int CustomerId {get; set;}
    public int PurchaseOrderId {get; set;}
    public string ExperienceTitle {get; set;}
    public int Status {get; set;}
    public string Reason {get; set;}
    public AssociatedCustomer Customer {get; set;}
    public AssociatedPurchase PurchaseOrder {get; set;}
    public decimal RefundAmountGiven { get; set; }

    public class AssociatedCustomer 
    {
        public int Id {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Email {get; set; }
    }

    public class AssociatedPurchase 
    {
        public int Id {get; set;}
        public int ActivityId {get; set;}
        public int ScheduleId {get; set;}
        public decimal OverAllTotal {get; set;}
    }
}