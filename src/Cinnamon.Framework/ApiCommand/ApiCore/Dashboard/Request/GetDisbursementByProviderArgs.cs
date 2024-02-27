namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;

public class GetDisbursementByProviderArgs
{
    public int ProviderId { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public string? FilterBy { get; set; }
    public string? FilterValue { get; set; }
}
