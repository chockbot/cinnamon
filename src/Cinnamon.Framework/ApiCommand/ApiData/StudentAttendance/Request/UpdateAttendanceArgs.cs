using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;

public class UpdateAttendanceArgs 
{
    [Required]
    public IEnumerable<UpdateAttendance> StudentAttendaces {get; set;}
    [Required]
    public DateTime Date {get; set;}

    public DateTime ExpirationStartDate { get; set;}
    public DateTime ExpirationEndDate { get; set;}

    public class UpdateAttendance 
    {
        [Required]
        public int StudentId {get; set;}
        [Required]
        public bool IsPresent {get; set;}
    }
}