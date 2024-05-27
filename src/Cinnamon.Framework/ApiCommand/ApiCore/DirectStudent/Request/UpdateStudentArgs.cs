using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;

public class UpdateStudentArgs
{
    [Required]
    public int StudentId {get; set;}

    public string? Name {get; set;}

    public string? Gender {get; set;}

    public string? BirthMonth {get; set;}

    public int? BirthYear {get; set;}

    public int? ActivityId {get; set;}

    public int? ScheduleId {get; set;}

    public string? StudentNo {get; set;}

    public string? Remarks {get; set;}

    public decimal? Amount {get; set;}

    public int? NumberOfSessions {get; set;}
}