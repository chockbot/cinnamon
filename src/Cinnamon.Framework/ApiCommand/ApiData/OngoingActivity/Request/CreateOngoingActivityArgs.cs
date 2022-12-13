using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OngoingActivity.Request;

public class CreateOngoingActivityArgs 
{
    [Required]
    public int ActivityId {get; set;}
    [Required]
    public int CustomerId {get; set;}
    [Required]
    public int PurchaseOrderId {get; set;}
}