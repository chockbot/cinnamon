namespace Cinnamon.Api.Data.Repository.Entities;

public class DisbursementBulk : BaseEntity 
{
    public int CustomerId {get; set;}
    public decimal Amount {get; set;}
    public string Status {get; set;} // pending|disbursed
    public string Remarks {get; set;}

    public virtual IList<DisbursementDetailBulk> DisbursementDetailBulks {get; set;}
}