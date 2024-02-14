using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;

public class CreateDisbursementBulkLogArgs 
{
    [Required]
    public DisbursementBulkLogArgs DisbursementBulkLog {get; set;}

    public class DisbursementBulkLogArgs 
    {
        [Required]
        public int DisbursementBulkId {get; set;}

        [Required]
        public string RefferenceId {get; set;}

        [Required]
        public string Status {get; set;}

        public string? Remarks {get; set;} = string.Empty;
    }
}