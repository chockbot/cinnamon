using Cinnamon.Web.Models.Entities;
namespace Cinnamon.Web.Models.Maker;

public class ReportModel
{
    public CustomerProfile profile                    = new CustomerProfile();
    public List<Students> enrollees                   = new();
    public List<Students> FilteredEnrollees           = new();
    public List<Students> ExportEnrollees             = new();
    public List<StudentAttendance> studentAttendances = new();
    public string Token                               = string.Empty;
    public int pageItems      = 10;
    public string currentPage = "1";
    public string monthName   = "";
    public int monthsAway     = 0;
    public int numDummyColumn = 0;
    public int year           = 2023;
    public int month          = 0;
    public DateTime passDates;
    public DateTime monthEnd;
    public bool disableButtonRight = false;
    public bool disableButtonLeft = false;
    
}
