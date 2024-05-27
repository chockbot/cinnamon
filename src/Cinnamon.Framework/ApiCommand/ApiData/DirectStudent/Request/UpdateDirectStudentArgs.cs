using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;

public class UpdateDirectStudentArgs 
{
    [Required]
    public UpdateDirectStudent UpdateDirectStudentData {get; set;}

    public class UpdateDirectStudent
    {
        [Required]
        public UpdateDirectStudentInfo UpdateDirectStudentInfo {get; set;}

        [Required]
        public UpdateDirectStudentSession UpdateDirectStudentSession {get; set;}

        [Required]
        public UpdateDirectStudentPayment UpdateDirectStudentPayment {get; set;}
    }

    public class UpdateDirectStudentInfo 
    {
        [Required]
        public int Id {get; set;}
        
        public string? Name {get; set;}

        public string? Gender {get; set;}

        public string? BirthMonth {get; set;}

        public int? BirthYear {get; set;}
    }

    public class UpdateDirectStudentSession 
    {
        public int? ActivityId {get; set;}

        public int? ScheduleId {get; set;}

        public string? Name {get; set;}

        public int? NumberOfSessions {get; set;}

        public string? StudentNo {get; set;}

        public string? Remarks {get; set;}
    }

    public class UpdateDirectStudentPayment 
    {
        public decimal? Amount {get; set;}
    }
}