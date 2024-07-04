namespace Cinnamon.Web.Models.Entities;
public class DirectStudents
{
    public CreateDirectStudent CreateDirectStudents { get; set; }
    public bool IsNew { get; set; } = false;

    public class CreateDirectStudent
    {
        public CreateDirectStudentInfo CreateDirectStudentInfo { get; set; }

        public CreateDirectStudentSession CreateDirectStudentSession { get; set; }

        public CreateDirectStudentPayment CreateDirectStudentPayment { get; set; }
    }

    public class CreateDirectStudentInfo
    {
        public int ProviderId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string BirthMonth { get; set; } = string.Empty;

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

        public string Remarks { get; set; }

        public string Status { get; set; }
    }

    public class CreateDirectStudentPayment
    {
        public decimal Amount { get; set; }
    }
}
