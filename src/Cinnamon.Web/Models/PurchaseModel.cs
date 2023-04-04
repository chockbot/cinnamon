using Cinnamon.Web.Models.Entities;
namespace Cinnamon.Web.Models;

public class PurchaseModel
{
    public Activity Activity { get; set; }
    public ActivitySchedule SelectedSchedule {get; set;}
    public CustomerProfile Customer {get; set;}
    public string Token { get; set; } = string.Empty;
    public bool HasError { get; set; }
    public bool IsCreditApplied {get; set;}
}
