using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Student.Request;

public class UpdateStudentArgs 
{
    [Required]
    public int StudentId {get; set;}
    public string? Name {get; set;}
    public string? StudentNo {get; set;}
    public int? NumberOfSessions {get; set;}
    public int? SessionsAttended {get; set;}
    public int? NumberOfBacktracking { get; set; }
    public string? Remarks {get; set;}
    public string? Status {get; set;}
    public DateTime? ExpirationStartDate { get; set; }
    public DateTime? ExpirationEndDate { get; set; }
    public bool? HasReview { get; set; }
}