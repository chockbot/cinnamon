using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class RequestRefundArgs
{
    [Required]
    public int PurchaseOrderId {get; set;}
    [Required]
    public string Reason {get; set;}
}