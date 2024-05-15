using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;

public class GetEnrolledStudentsByProviderArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    [Required]
    public int ProviderId { get; set; }
    public string? SearchValue { get; set; }
    public int? SearchBy { get; set; }
}
