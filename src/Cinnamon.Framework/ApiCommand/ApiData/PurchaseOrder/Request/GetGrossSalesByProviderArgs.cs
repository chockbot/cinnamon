using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Request;

public class GetGrossSalesByProviderArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    [Required]
    public int Id { get; set; }
    [Required]
    public string DateFrom { get; set; }
    // date format must yyyyMMddHHmmss
}
