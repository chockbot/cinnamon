using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Student.Request;

public class CreateManyStudentArgs 
{
    [Required]
    public int CustomerId {get; set;}
    [Required]
    public int ActivityId {get; set;}
    [Required]
    public int ScheduleId {get; set;}
    [Required]
    public int NumberOfSessions {get; set;}
    public int SessionsAttended {get; set;} = 0;
    public DateTime ExpirationStartDate { get; set; }
    public DateTime ExpirationEndDate { get; set; }
    [Required]
    public IEnumerable<StudentDetails> Students {get; set;}

    public class StudentDetails 
    {
        [Required]
        public int FamilyMemberId {get; set;}
        [Required]
        public string Name {get; set;}
        [Required]
        public string StudentNo {get; set;}
    }
}