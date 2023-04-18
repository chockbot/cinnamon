using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Request;

public class UpdatePurchaseOrdersStatusArgs 
{
    [Required]
    public IEnumerable<int> Ids {get; set;}
    [Required]
    public int Status {get; set;}
}