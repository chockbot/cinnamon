using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.RequestRefund.Request;

public class CreateRequestRefundArgs 
{
    [Required]
    public int CustomerId {get; set;}
    [Required]
    public int PurchaseOrderId {get; set;}
    [Required]
    public string ExperienceTitle {get; set;}
    [Required]
    public int Status {get; set;}
    [Required]
    public string Reason {get; set;}
}