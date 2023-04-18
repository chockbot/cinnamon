using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;

public class UpdateStudentAttedanceCurrentDateArgs : IInteractor
{
    public IEnumerable<StudentDetails> Students {get; set;}

    public class StudentDetails 
    {
        public int StudentId {get; set;}
        public int ActivityId {get; set;}
        public int ScheduleId {get; set;}
        public bool IsPresent {get; set;}
    }
}