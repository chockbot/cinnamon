using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;

public class CreateManualDisbursementArgs 
{
    [Required]
    public int DisbursementId {get; set;}

    [Required]
    public int AdminId {get; set;}

    [Required]
    public string Reason {get; set;}

}