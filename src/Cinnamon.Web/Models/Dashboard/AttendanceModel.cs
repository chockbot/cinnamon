namespace Cinnamon.Web.Models.Dashboard;

public class AttendanceModel 
{
    public List<Schedule> Schedules {get; set;} = new();
    public List<StudentAttendance> Attendances {get; set;} = new();
    public bool IsShowErrorMessage {get; set;}
    public string ErrorMessage {get; set;}
    public bool IsCheckAllStudent {get; set;}

    public class Schedule 
    {
        public int ActivityId {get; set;}
        public string ActivityTitle {get; set;}
        public string ActivityDescription {get; set;}
        public int ScheduleId {get; set;}
        public string ScheduleTitle {get; set;}
        public string ScheduleDescription {get; set;}
        public bool IsSelected {get; set;}
    }

    public class StudentAttendance 
    {
        public int StudentId {get; set;}
        public string Name {get; set;}
        public bool IsPresent {get; set;}
        public string ActivityName {get; set;}
    }
}

