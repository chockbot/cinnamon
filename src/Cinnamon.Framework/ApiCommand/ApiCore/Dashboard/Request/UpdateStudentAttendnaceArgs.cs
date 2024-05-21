using System.ComponentModel.DataAnnotations;
using Cinnamon.Framework.Enums;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;

public class UpdateStudentAttendnaceArgs 
{
    public IEnumerable<StudentToUpdate> Students {get; set;}

    public class StudentToUpdate 
    {
        [Required]
        public int StudentId {get; set;}
        [Required]
        public int ActivityId {get; set;}
        [Required]
        public int ScheduleId {get; set;}
        [Required]
        public bool IsPresent {get; set;}

        [Required]
        public int SessionAttended { get; set; }

        [Required]
        public StudentType StudentType {get; set;} = StudentType.Cinnamon;
    }
}