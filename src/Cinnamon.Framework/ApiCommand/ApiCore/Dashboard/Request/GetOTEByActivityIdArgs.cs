using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;

public class GetOTEByActivityIdArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    [Required]
    public int ActivityId { get; set; }
    public string? SearchValue { get; set; }
}
