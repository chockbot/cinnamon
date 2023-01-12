using Cinnamon.Web.Models.Entities;
namespace Cinnamon.Web.Models;

public class PurchaseModel
{
    public Activity Activity { get; set; }
    public ActivitySchedule SelectedSchedule {get; set;}
    public CustomerProfile Customer {get; set;}
    public IList<FamilyMember> SelectedAttendees {get; set;} = new List<FamilyMember>();
    public string Token { get; set; } = string.Empty;
    public bool HasError { get; set; }
}
