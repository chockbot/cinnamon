using Blazorise;
using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models.Dashboard;

public class ManageStudentsModel
{
    public Models.Customer.Profile Model;

    public List<Activity> activities = new List<Activity>();

    public List<Students> students = new List<Students>();

    public List<ActivitySchedule> activitySchedules = new List<ActivitySchedule>();

    public List<ManageStudentsData> manageStudents = new List<ManageStudentsData>();

    public List<StudentAttendance> studentAttendances = new List<StudentAttendance>();
    public List<ManageStudentsData> FilteredStudents { get; set; } = new();

    public bool? IsSortNameAsc { get; set; }

    public bool? IsSortExperienceAsc { get; set; }

    public string customFilterValue;

    public int selectedFilter;

    public bool isDisplay = false;

    public int searchBy;

    public TextEdit textEdit;

    public bool showErrorMessage = false;

    public bool showSuccessMessage = false;

    public string errorMessage = string.Empty;

    public int jerseyNumber;

    public int Id;

    public int pageItems = 20;

    public string currentPage = "1";
}
