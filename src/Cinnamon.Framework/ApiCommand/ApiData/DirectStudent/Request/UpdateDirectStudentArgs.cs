using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;

public class UpdateDirectStudentArgs 
{
    public UpdateDirectStudentInfo? UpdateStudentInfo {get; set;}

    public IEnumerable<UpdateDirectStudentSession>? UpdateStudentSessions {get; set;}

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
        [Required]
        public int Id {get; set;}

        public int? ActivityId {get; set;}

        public int? ScheduleId {get; set;}

        public string? Name {get; set;}

        public int? NumberOfSessions {get; set;}

        public int? SessionsAttended { get; set; }

        public string? StudentNo {get; set;}

        public string? Remarks {get; set;}

        public UpdateDirectStudentPayment? DirectStudentPayment {get; set;}
    }

    public class UpdateDirectStudentPayment 
    {
        public int Id {get; set;}

        public decimal? Amount {get; set;}
    }
}