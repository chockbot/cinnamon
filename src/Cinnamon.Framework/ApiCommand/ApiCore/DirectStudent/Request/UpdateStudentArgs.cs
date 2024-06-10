using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;

public class UpdateStudentArgs
{
    public UpdateDirectStudentInfo? DirectStudentInfo {get; set;}
    
    public IEnumerable<UpdateDirectStudentSession>? DirectStudentSessions {get; set;}

    public class UpdateDirectStudentInfo 
    {
        [Required]
        public int StudentId {get; set;}

        public string? Name {get; set;}
        public string? Gender {get; set;}
        public string? BirthMonth {get; set;}
        public int? BirthYear {get; set;}       
    }

    public class UpdateDirectStudentSession 
    {
        [Required]
        public int Id {get; set;}
        
        public string? Name {get; set;}
        public int? ActivityId {get; set;}
        public int? ScheduleId {get; set;}
        public int? NumberOfSessions {get; set;}
        public int? SessionsAttended { get; set; }
        public string? StudentNo {get; set;}
        public string? Remarks {get; set;}

        public UpdateDirectStudentPayment? StudentPayment {get; set;}
    }

    public class UpdateDirectStudentPayment 
    {
        public decimal? Amount {get; set;}
    }

}