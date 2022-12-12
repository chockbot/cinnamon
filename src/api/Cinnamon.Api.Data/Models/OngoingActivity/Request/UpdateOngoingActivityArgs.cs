using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.OngoingActivity.Request;

public class UpdateOngoingActivityArgs 
{
    [Required]
    public int OngoingActivityId {get; set;}
    public int? ActivityId {get; set;}
    public int? CustomerId {get; set;}
    public int? PurchaseOrderId {get; set;}
}