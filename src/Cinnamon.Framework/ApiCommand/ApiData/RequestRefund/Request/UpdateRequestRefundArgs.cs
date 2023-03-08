using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.RequestRefund.Request;

public class UpdateRequestRefundArgs 
{
    [Required]
    public int RefundId {get; set;}
    [Required]
    public int Status {get; set;}
}