using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;

public class UpdateDisbursementBulkStatusArgs 
{
    [Required]
    public int DisbursementBulkId {get; set;}

    [Required]
    public string DisbursementBulkStatus {get; set;}

    [Required]
    public string DisbursementStatus {get; set;}

    public string? Remarks {get; set;} = string.Empty;

}