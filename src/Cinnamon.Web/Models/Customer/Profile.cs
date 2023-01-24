using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models.Customer;

public class Profile 
{
    public CustomerProfile CustomerProfile { get; set; }
    public IList<FamilyMember> FamilyMembers {get; set;}
    public IList<Activity> OwnedActivities {get; set;}
    public IList<Activity> EnrolledActivities {get; set;}
    public string Token { get; set; }
}