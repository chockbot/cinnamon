namespace Cinnamon.Api.Data.Repository.Entities;

public class DisbursementBulkLog : BaseEntity 
{
    public int DisbursementBulkId {get; set;}
    public string RefferenceId {get; set;}
    public string Status {get; set;} // pending|success|error|expired
    public string Remarks {get; set;}
}