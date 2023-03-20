using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.PayoutLog.Request;

public class CreatePayoutLogArgs 
{
    [Required]
    public int PurchaseOrderId {get; set;}
    [Required]
    public int CustomerId {get; set;}
    [Required]
    public decimal Amount {get; set;}
    [Required]
    public int Status {get; set;}
    [Required]
    public string Remarks {get; set;}   
}