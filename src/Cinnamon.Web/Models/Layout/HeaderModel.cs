using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models.Layout;

public class HeaderModel 
{
    public CustomerProfile Profile {get; set;}
    public bool IsAuthenticated {get; set;}
}