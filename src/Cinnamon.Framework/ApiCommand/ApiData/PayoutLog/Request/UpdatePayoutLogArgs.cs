using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.PayoutLog.Request;

public class UpdatePayoutLogArgs
{
    [Required]
    public int Id {get; set;}
    public int? Status {get; set;}
    public string? Remarks {get; set;}
}