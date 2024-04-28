using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;

public class GetPayoutsByProviderArgs
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string DateFrom { get; set; }
    [Required]
    public int Status { get; set; }
}
