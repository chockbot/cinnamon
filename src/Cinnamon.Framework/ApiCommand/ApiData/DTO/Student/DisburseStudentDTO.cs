namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;

public class DisburseStudentDTO 
{
    public int TransactionId {get; set;}
    public int MakerId {get; set;}
    public int StudentId {get; set;}
    public int ActivityId {get; set;}
    public int UnitCount {get; set;}
    public decimal UnitPrice {get; set;}
    public string Name {get; set;}
    public int NumberOfSessions {get; set;}
    public int SessionsAttended {get; set;}
}