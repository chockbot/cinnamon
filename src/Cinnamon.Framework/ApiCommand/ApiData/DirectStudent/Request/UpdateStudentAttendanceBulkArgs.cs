using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;

public class UpdateStudentAttendanceBulkArgs 
{
    [Required]
    public IEnumerable<UpdateStudentAttendance> StudentAttendances {get; set;}

    [Required]
    public DateTime Date {get; set;}

    public class UpdateStudentAttendance
    {
        [Required]
        public int DirectStudentSessionId {get; set;}

        [Required]
        public bool IsPresent {get; set;}
    }
}