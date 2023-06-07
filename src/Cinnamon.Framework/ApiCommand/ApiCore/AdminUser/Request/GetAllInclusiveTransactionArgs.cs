namespace Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;

public class GetAllInclusiveTransactionArgs 
{
    public string? Name {get; set;}

    // date format must be yyyyMMddHHmmss
    public string? PurchaseDateFrom {get; set;}

    // date format must be yyyyMMddHHmmss
    public string? PurchaseDateTo {get; set;}

    public string? Email {get; set;}

    public int? Status {get; set;}
}