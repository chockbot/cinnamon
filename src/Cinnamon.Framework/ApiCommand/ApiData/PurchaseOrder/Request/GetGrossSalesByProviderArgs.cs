namespace Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Request;

public class GetGrossSalesByProviderArgs
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int Id { get; set; }
    public DateTime DateFrom { get; set; }
}
