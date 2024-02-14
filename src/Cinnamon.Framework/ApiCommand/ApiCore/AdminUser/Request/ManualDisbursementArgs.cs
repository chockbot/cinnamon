using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;

public class ManualDisbursementArgs 
{
    [Required]
    public int DisbursementId {get; set;}

    [Required]
    public string Remarks {get; set;}
}