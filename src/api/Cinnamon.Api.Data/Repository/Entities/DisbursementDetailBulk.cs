namespace Cinnamon.Api.Data.Repository.Entities;

public class DisbursementDetailBulk : BaseEntity 
{
    public int DisbursementBulkId {get; set;}
    public int DisbursementId {get; set;}
    public decimal Amount {get; set;}

    public virtual DisbursementBulk DisbursementBulk {get; set;}
}