namespace Cinnamon.Api.Data.Repository.Entities;

public class Disbursement : BaseEntity 
{
    public int PurchaseOrderId {get; set;}
    public int CustomerId {get; set;}
    public string Label {get; set;}
    public decimal Amount {get; set;}
    public string Status {get; set;} // initiated|pending|disbursed
    public string Remarks {get; set;}
    public bool InclusivePayment {get; set;}
    public string Payload {get; set;}

    public virtual IList<DisbursementDetail> DisbursementDetails {get; set;}
}