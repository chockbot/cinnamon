namespace Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;

public class UpdateStudentAttendanceResult 
{
    public IEnumerable<UpdatedStudentDetails> StudentAttendaces {get; set;}

    public class UpdatedStudentDetails 
    {
        public int StudentId {get; set;}
        public int ActivityId {get; set;}
        public int ScheduleId {get; set;}
        public bool IsPresent {get; set;}
    }
}