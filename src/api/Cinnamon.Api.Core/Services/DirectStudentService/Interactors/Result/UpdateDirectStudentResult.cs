namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;

public class UpdateDirectStudentResult
{
    public int Id {get; set;}
    public int ProviderId {get; set;}
    public string Name {get; set;}
    public string Gender {get; set;}
    public string BirthMonth {get; set;}
    public int BirthYear {get; set;}

    public int ActivityId {get; set;}
    public int ScheduleId {get; set;}

    public decimal Amount {get; set;}
}
