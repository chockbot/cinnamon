namespace Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;

public class UpdateStudentArgs
{
    public int StudentId {get; set;}
    public string Name {get; set;}
    public string Gender {get; set;}
    public string BirthMonth {get; set;}
    public int BirthYear {get; set;}

    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}

    public decimal Amount {get; set;}
}