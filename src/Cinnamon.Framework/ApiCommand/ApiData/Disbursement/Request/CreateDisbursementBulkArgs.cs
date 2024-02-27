using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;

public class CreateDisbursementBulkArgs 
{

    [Required]
    public DisbursementBulkArgs DisbursementBulk {get; set;}

    public class DisbursementBulkArgs 
    {
        [Required]
        public int CustomerId {get; set;}

        [Required]
        public decimal Amount {get; set;}

        [Required]
        public string Status {get; set;}

        public string? Remarks {get; set;} = string.Empty;

        [Required]
        public IEnumerable<DisbursementDetailBulkArgs> DisbursementDetailBulks {get; set;}
    }

    public class DisbursementDetailBulkArgs 
    {
        [Required]
        public int DisbursementId {get; set;}

        [Required]
        public decimal Amount {get; set;}
    }
}