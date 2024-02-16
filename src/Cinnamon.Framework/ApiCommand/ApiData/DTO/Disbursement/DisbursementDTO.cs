namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.Disbursement;

public class DisbursementDTO 
{
    public int Id {get; set;}
    public int PurchaseOrderId {get; set;}
    public int CustomerId {get; set;}
    public string Label {get; set;}
    public decimal Amount {get; set;}
    public string Status {get; set;}
    public string Remarks {get; set;}
    public bool InclusivePayment {get; set;}
    public string Payload {get; set;}

    public IEnumerable<DisbursementDetailDTO> DisbursementDetails {get; set;}
}