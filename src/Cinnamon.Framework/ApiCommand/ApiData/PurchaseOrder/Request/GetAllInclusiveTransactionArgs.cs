namespace Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Request;

public class GetAllInclusiveTransactionArgs 
{
    public string? Name {get; set;}

    // date format must be yyyyMMddHHmmss
    public string? PurchaseDate {get; set;}

    public string? Email {get; set;}

    public int? Status {get; set;}
}