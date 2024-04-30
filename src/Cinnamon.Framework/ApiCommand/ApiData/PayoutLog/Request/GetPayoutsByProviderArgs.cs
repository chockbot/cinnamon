using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.PayoutLog.Request;

public class GetPayoutsByProviderArgs
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string DateFrom { get; set; }
    // date format must yyyyMMddHHmmss
    [Required]
    public int Status { get; set; }
}
