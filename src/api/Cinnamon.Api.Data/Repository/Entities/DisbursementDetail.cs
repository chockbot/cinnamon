namespace Cinnamon.Api.Data.Repository.Entities;

public class DisbursementDetail : BaseEntity 
{
    public int DisbursementId {get; set;}
    public string Label {get; set;}
    public string Amount {get; set;}

    public virtual Disbursement Disbursement {get; set;}
}