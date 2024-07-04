using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;

public class GetDirectStudentSalesArgs
{
    [Required]
    public int ProviderId { get; set; }
    [Required]
    public string DateFrom { get; set; }
}
