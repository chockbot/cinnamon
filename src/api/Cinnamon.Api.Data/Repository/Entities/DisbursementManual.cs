namespace Cinnamon.Api.Data.Repository.Entities;

public class DisbursementManual : BaseEntity 
{
    public int DisbursementId {get; set;}
    public int AdminId {get; set;}
    public string Reason {get; set;}
}