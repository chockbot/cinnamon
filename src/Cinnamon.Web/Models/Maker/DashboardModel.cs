using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models.Maker;

public class DashboardModel 
{
    public IEnumerable<Activity> Activities {get; set;}
    public int ActiveActivityCount {get; set;}
    public string Token {get; set;}
}