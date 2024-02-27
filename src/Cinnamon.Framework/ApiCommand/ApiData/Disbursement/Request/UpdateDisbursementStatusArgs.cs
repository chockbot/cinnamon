using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;

public class UpdateDisbursementStatusArgs 
{
    [Required]
    public int DisbursementId {get; set;}

    [Required]
    public string Status {get; set;}

    [Required]
    public string Remarks {get; set;}

}