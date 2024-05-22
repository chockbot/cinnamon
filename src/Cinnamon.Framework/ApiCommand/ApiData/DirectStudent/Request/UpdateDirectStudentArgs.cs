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
        public string Name {get; set;}

        [Required]
        public string Gender {get; set;}

        [Required]
        public string BirthMonth {get; set;}

        [Required]
        public int BirthYear {get; set;}
    }

    public class UpdateDirectStudentSession 
    {
        [Required]
        public int ActivityId {get; set;}

        [Required]
        public int ScheduleId {get; set;}

        [Required]
        public string Name {get; set;}

        [Required]
        public string StudentNo {get; set;}

        [Required]
        public int NumberOfSessions {get; set;}

        [Required]
        public int SessionsAttended {get; set;}

        public string Remarks {get; set;}

        [Required]
        public string Status {get; set;}
    }

    public class UpdateDirectStudentPayment 
    {
        [Required]
        public decimal Amount {get; set;}
    }
}