namespace Cinnamon.Framework.ApiCommand.ApiData.Disbursement.Request;

public class GetDisbursementsArgs 
{
    public string? Status {get; set;}
    public int? Count {get; set;}
    public int? Skip {get; set;}
}