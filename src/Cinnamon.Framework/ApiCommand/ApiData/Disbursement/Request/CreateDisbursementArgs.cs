using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;

public class CreateDisbursementArgs 
{
    [Required]
    public IEnumerable<DisbursementArgs> Disbursements {get; set;}
    public IEnumerable<int>? StudentIds {get; set;}

    public class DisbursementArgs
    {
        [Required]
        public int PurchaseOrderId {get; set;}

        [Required]
        public int CustomerId {get; set;}

        [Required]
        public string Label {get; set;}

        [Required]
        public decimal Amount {get; set;}

        [Required]
        public string Status {get; set;}

        public string? Remarks {get; set;} = string.Empty;

        [Required]
        public bool InclusivePayment {get; set;}

        public string? Payload {get; set;} = string.Empty;

        [Required]
        public IEnumerable<DisbursementDetailArgs> DisbursementDetails {get; set;}
    }

    public class DisbursementDetailArgs 
    {
        [Required]
        public string Label {get; set;}

        [Required]
        public decimal Amount {get; set;}
    }
}