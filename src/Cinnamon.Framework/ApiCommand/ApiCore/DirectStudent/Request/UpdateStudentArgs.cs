using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;

public class UpdateStudentArgs
{
    [Required]
    public int StudentId {get; set;}

    [Required]
    public string Name {get; set;}

    [Required]
    public string Gender {get; set;}

    [Required]
    public string BirthMonth {get; set;}

    [Required]
    public int BirthYear {get; set;}

    [Required]
    public int ActivityId {get; set;}

    [Required]
    public int ScheduleId {get; set;}

    [Required]
    public decimal Amount {get; set;}
}