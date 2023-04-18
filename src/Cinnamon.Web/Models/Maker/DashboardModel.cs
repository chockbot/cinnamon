using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models.Maker;

public class DashboardModel 
{
    public IEnumerable<Activity> Activities {get; set;}
    public IEnumerable<CustomerProfile> Customers { get; set;}
    public IEnumerable<Students> Students { get; set;}
    public int ActiveActivityCount {get; set;}
    public string Token {get; set;}
    public bool IsPublished { get; set; }
}