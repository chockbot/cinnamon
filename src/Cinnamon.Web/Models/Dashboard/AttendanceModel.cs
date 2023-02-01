namespace Cinnamon.Web.Models.Dashboard;

public class AttendanceModel 
{
    public IList<Schedule> Schedules {get; set;} = new List<Schedule>();

    public class Schedule 
    {
        public int Id {get; set;}
        public string Title {get; set;}
        public string Description {get; set;}
        public bool IsSelected {get; set;}
    }
}

