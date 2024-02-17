namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.Disbursement;

public class DisbursementBulkDTO 
{
    public int Id {get; set;}
    public int CustomerId {get; set;}
    public decimal Amount {get; set;}
    public string Status {get; set;}
    public string Remarks {get; set;}
    public IList<DisbursementDetailBulkDTO> DisbursementDetailBulks {get; set;}
}