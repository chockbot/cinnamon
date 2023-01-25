namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.OngoingActivity;

public class OngoingActivityDTO 
{
    public int Id {get; set;}
    public int ActivityId {get; set;}
    public int CustomerId {get; set;}
    public int PurchaseOrderId {get; set;}

    public DTO.Activity.ActivityDTO Activity {get; set;}
}