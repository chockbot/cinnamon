namespace Cinnamon.Api.Data.Repository.Entities;

public class ExperienceType : BaseEntity 
{
    public string Name {get; set;}

    public virtual IList<Activity> Activities { get; set;}
}
