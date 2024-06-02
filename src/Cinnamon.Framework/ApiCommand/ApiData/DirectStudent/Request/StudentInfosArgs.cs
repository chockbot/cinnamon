using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;

public class StudentInfosArgs 
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }

    public string? Name {get; set;}

    [Required]
    public int ProviderId {get; set;}
}