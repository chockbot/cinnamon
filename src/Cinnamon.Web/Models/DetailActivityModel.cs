using Cinnamon.Web.Models.Entities;
namespace Cinnamon.Web.Models;
public class DetailActivityModel
{
    public Activity Activity { get; set; } = new();
    public IEnumerable<ExperienceType> ExperienceTypes { get; set; }
    public CustomerProfile Customer { get; set; }
    public bool HasError { get; set; }

}
