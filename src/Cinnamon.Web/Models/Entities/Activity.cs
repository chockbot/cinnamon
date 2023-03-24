namespace Cinnamon.Web.Models.Entities;

public class Activity 
{
    public int ActivityId {get; set;}
    public int ExperienceTypeId {get; set;}
    public int ExperienceCategoryId {get; set;}
    public int SubCategoryId {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public string Price {get; set;}
    public string ScheduleIndicator {get; set;} = " ";
    public string Remarks {get; set;} = " ";
    public bool IsPublished {get; set;}
    public string Address1 {get; set;} = string.Empty;
    public string Address2 {get; set;} = string.Empty;
    public string District {get; set;} = string.Empty;
    public string City {get; set;} = string.Empty;
    private string _cityName;

    public string CityName
    {
        get { return string.IsNullOrEmpty(_cityName) ? "" : _cityName; }
        set { _cityName = value; }
    }

    public string Subdivision { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    private string _regionName;
    public string RegionName
    {
        get { return string.IsNullOrEmpty(_regionName) ? "" : _regionName; }
        set { _regionName = value; }
    }
    public string Barangay { get; set; } = string.Empty;
    private string _barangayName;
    public string BarangayName
    {
        get { return string.IsNullOrEmpty(_barangayName) ? "" : _barangayName; }
        set { _barangayName = value; }
    }
    public string PostalCode { get; set; } = string.Empty;
    public string SpecificsYouWillProvide {get; set;}
    public string CustomerBringWithThem {get; set;}
    public string? AdditionalRequirements {get; set;}
    public string ActivityLevel {get; set;}
    public string SkillLevel {get; set;}
    public int MinimumAge {get; set;}
    public bool CanAdultsJoin {get; set;}
    public int CreatedBy { get; set; }
    public string MapDetails { get; set; }
    public string Handler {get; set;}
    public string ExperienceType { get; set; }
    public string ExperienceCategory { get; set; }
    public string SubCategory { get; set; }
    public bool IsSetSession { get; set; }
    public string SessionName { get; set; }
    public string PinnedLocation { get; set; }
    public IList<string> SearchTags {get; set;} = new List<string>();
    public IList<ActivitySchedule> ActivitySchedules {get; set;} = new List<ActivitySchedule>();
    public IList<ActivityImage> Images {get; set;}
    public CustomerProfile? Owner {get; set;}
    public bool IsNew { get; set; }
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public int OngoingStudents { get; set; }
    public int CompletedStudents { get; set; }
    public bool IsDeactivated { get; set; }
}