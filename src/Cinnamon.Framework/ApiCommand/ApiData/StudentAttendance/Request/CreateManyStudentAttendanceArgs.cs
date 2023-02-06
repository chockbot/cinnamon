using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;

public class CreateManyStudentAttendanceArgs 
{
    [Required]
    public IEnumerable<StudentAttendaceDetails> StudentAttendaces {get; set;}

    public class StudentAttendaceDetails 
    {
        [Required]
        public int StudentId {get; set;}
        [Required]
        public DateTime Date {get; set;}
        [Required]
        public bool IsPresent {get; set;}
    }
}