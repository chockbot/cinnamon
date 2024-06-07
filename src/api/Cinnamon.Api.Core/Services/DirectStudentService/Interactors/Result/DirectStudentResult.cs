namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;

public class DirectStudentResult
{
    public DirectStudentInfo DirectStudentInfoResult {get; set;}
    public DirectStudentSession DirectStudentSessionResult {get; set;}
    public DirectStudentPayment DirectStudentPaymentResult {get; set;}

    public class DirectStudentInfo 
    {
        public int Id {get; set;}
        public int ProviderId {get; set;}
        public string Name {get; set;}
        public string Gender {get; set;}
        public string BirthMonth {get; set;}
        public int BirthYear {get; set;}
        public int Age {get; set;}
    }

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
    }

    public class DirectStudentPayment 
    {
        public int Id {get; set;}
        public int DirectStudentSessionId {get; set;}
        public decimal Amount {get; set;}
        public DateTime CreatedOn { get; set; }
    }
}
