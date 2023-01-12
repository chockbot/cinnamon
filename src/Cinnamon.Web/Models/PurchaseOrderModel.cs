using Cinnamon.Web.Models.Entities;
namespace Cinnamon.Web.Models;

public class PurchaseOrderModel
{
    public ActivitySchedule SelectedSchedule {get; set;}
    public PurchaseOrder PurchaseOrder {get; set;}
    public string Token { get; set; } = string.Empty;
}
