using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class GetActivityArgs 
{
    public bool? IncludeAtivitySchedules {get; set;}
    public bool? IncludeActivityAddress {get; set;}
    public bool? IncludeActivityDescription {get; set;}
    public bool? IncludeActivitySearchTags {get; set;}
    public bool? IncludeActivityImages {get; set;}
    public bool? IsActive {get; set;}
    public bool? IncludeCustomer {get; set;}
    public bool? IncludeStudents{ get; set; }
}