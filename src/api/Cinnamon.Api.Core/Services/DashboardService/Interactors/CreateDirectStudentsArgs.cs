using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Interactors;
public class CreateDirectStudentsArgs : IInteractor
{
    public IEnumerable<CreateDirectStudent> CreateDirectStudents { get; set; }

    public class CreateDirectStudent
    {
        public CreateDirectStudentInfo CreateDirectStudentInfo { get; set; }

        public CreateDirectStudentSession CreateDirectStudentSession { get; set; }

        public CreateDirectStudentPayment CreateDirectStudentPayment { get; set; }
    }

    public class CreateDirectStudentInfo
    {
        public int ProviderId { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string BirthMonth { get; set; }

        public int BirthYear { get; set; }
    }

    public class CreateDirectStudentSession
    {
        public int ActivityId { get; set; }

        public int ScheduleId { get; set; }

        public string Name { get; set; }

        public string StudentNo { get; set; }

        public int NumberOfSessions { get; set; }

        public int SessionsAttended { get; set; }

        public string? Remarks { get; set; }

        public string Status { get; set; }
    }

    public class CreateDirectStudentPayment
    {
        public decimal Amount { get; set; }
    }
}
