namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;

public class StudentSessionsResult
{
    public IEnumerable<DirectStudentSession> DirectStudentSessions {get; set;}

    public class DirectStudentSession 
    {
        public int Id {get; set;}
        public int DirectStudentInfoId {get; set;}
        public int ActivityId {get; set;}
        public int ScheduleId {get; set;}
        public string Name {get; set;}
        public string StudentNo {get; set;}
        public int NumberOfSessions {get; set;}
        public int SessionsAttended {get; set;}
        public string Remarks {get; set;}
        public string Status {get; set;}

        public DirectStudentSessionPayment DirectStudentPayment {get; set;}
    }

    public class DirectStudentSessionPayment 
    {
        public int Id {get; set;}
        public decimal Amount {get; set;}
        public DateTime PaymentDate {get; set;}
    }
}
