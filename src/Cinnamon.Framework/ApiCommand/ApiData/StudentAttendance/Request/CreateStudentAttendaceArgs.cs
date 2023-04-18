using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;

public class CreateStudentAttendaceArgs 
{
    [Required]
    public int StudentId {get; set;}
    [Required]
    public bool IsPresent {get; set;}
    [Required]
    public DateTime Date {get; set;}
}