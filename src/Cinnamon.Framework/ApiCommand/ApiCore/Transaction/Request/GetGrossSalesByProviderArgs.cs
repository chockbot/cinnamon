using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;

public class GetGrossSalesByProviderArgs
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string DateFrom { get; set; }
}
