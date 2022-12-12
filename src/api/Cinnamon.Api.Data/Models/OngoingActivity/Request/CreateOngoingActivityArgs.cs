using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.OngoingActivity.Request;

public class CreateOngoingActivityArgs 
{
    [Required]
    public int ActivityId {get; set;}
    [Required]
    public int CustomerId {get; set;}
    [Required]
    public int PurchaseOrderId {get; set;}
}