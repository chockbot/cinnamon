namespace Cinnamon.Web.Models.Entities;

public class ManageStudentsData
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public int CustomerId { get; set; }
    public int ScheduleId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string StudentNo { get; set; }
    public string Remarks { get; set; }
    public int NumberOfSessions { get; set;}
    public int SessionsAttended { get; set; }
    public string Status { get; set; }
}
