using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;

public class UpdateManyStudentAttendanceArgs 
{
    [Required]
    public IEnumerable<UpdateStudentAttendaceDetails> StudentAttendaces {get; set;}

    public class UpdateStudentAttendaceDetails 
    {
        [Required]
        public int AttendanceId {get; set;}
        [Required]
        public DateTime Date {get; set;}
        [Required]
        public bool IsPresent {get; set;}
    }
}