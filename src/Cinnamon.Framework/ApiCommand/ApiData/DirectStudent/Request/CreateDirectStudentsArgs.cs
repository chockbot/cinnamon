using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;

public class CreateDirectStudentsArgs 
{
    [Required]
    public IEnumerable<CreateDirectStudent> CreateDirectStudents {get; set;}

    public class CreateDirectStudent
    {
        [Required]
        public CreateDirectStudentInfo CreateDirectStudentInfo {get; set;}

        [Required]
        public CreateDirectStudentSession CreateDirectStudentSession {get; set;}

        [Required]
        public CreateDirectStudentPayment CreateDirectStudentPayment {get; set;}
    }

    public class CreateDirectStudentInfo 
    {
        [Required]
        public int ProviderId {get; set;}
        
        [Required]
        public string Name {get; set;}

        [Required]
        public string Gender {get; set;}

        [Required]
        public string BirthMonth {get; set;}

        [Required]
        public int BirthYear {get; set;}
    }

    public class CreateDirectStudentSession 
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

    public class CreateDirectStudentPayment 
    {
        [Required]
        public decimal Amount {get; set;}
    }
}