using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Security;
using System.Text.RegularExpressions;

namespace Cinnamon.Web.Models.Entities;

public class Activity 
{
    public int ActivityId {get; set;}
    public int ExperienceTypeId {get; set;}
    public int ExperienceCategoryId {get; set;}
    public int SubCategoryId {get; set;}
    private string _title;
    public bool IsFavorite { get; set; }
    public string Title
    {
        get
        {
            if (!string.IsNullOrEmpty(_title))
            {
                string input = _title;
                string result = MaskEmail(input);

                result = MaskPhone(result);

                return result;
            }

            return _title;
        }
        set { _title = value; }
    }
    private string _description;
    public string Description
    {
        get {
            if (!string.IsNullOrEmpty(_description))
            {
                string input = _description;
                string result = MaskEmail(input);

                result = MaskPhone(result);

                return result;
            }

            return _description;
        }
        set { _description = value; }
    }

    public string Price {get; set;}
    public string ScheduleIndicator {get; set;} = " ";
    public string Remarks {get; set;} = " ";
    public bool IsPublished {get; set;}
    public bool ForceDisable {get; set;}

    public string IsPublishedDescription
    {
        get { return IsPublished ? "true" : "false"; }
    }

    private string _address1;

    public string Address1
    {
        get {
            if (!string.IsNullOrEmpty(_address1))
            {
                string input = _address1;
                string result = MaskEmail(input);

                result = MaskPhone(result);

                return result;
            }
            return _address1;
        }
        set { _address1 = value; }
    }

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
    private string _postalCode;

    public string PostalCode
    {
        get {
            if (!string.IsNullOrEmpty(_postalCode))
            {
                string input = _postalCode;
                string result = MaskEmail(input);

                result = MaskPhone(result);

                return result;
            }

            return _postalCode;
        }
        set { _postalCode = value; }
    }

    public string SpecificsYouWillProvide {get; set;}
    private string _customerBringWithThem;

    public string CustomerBringWithThem
    {
        get {
            if (!string.IsNullOrEmpty(_customerBringWithThem))
            {
                string input = _customerBringWithThem;
                string result = MaskEmail(input);

                result = MaskPhone(result);

                return result;
            }

            return _customerBringWithThem; 
        }
        set { _customerBringWithThem = value; }
    }
    private string _classPolicies;
    public string ClassPolicies
    {
        get
        {
            if (!string.IsNullOrEmpty(_classPolicies))
            {
                string input = _classPolicies;
                string result = MaskEmail(input);

                result = MaskPhone(result);

                return result;
            }

            return _classPolicies;
        }
        set { _classPolicies = value; }
    }

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
    public string VideoLink { get; set; }
    public DateTime CreatedOn { get; set; }
    public Cinnamon.Framework.Enums.Enums.ActivityStatus Status { get; set; }
    public Cinnamon.Framework.Enums.Enums.ExperienceCreationType ExperienceCreationType { get; set; }
    public Cinnamon.Framework.Enums.Enums.ScheduleType ScheduleType { get; set; }
    public string SchedulingUrl { get; set; }
    public int ActivityStatus { get { return (int)Status; } }
    public IList<string> SearchTags {get; set;} = new List<string>();
    public IList<ActivitySchedule> ActivitySchedules {get; set;} = new List<ActivitySchedule>();
    public IList<AddOn> AddOns { get; set;} = new List<AddOn>();
    public IList<ActivityImage> Images {get; set;}
    public CustomerProfile? Owner {get; set;}
    public bool IsNew { get; set; }
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public int OngoingStudents { get; set; }
    public int CompletedStudents { get; set; }
    public bool IsDeactivated { get; set; }
    public double AverageRating { get; set; }
    public int NumberOfReviews { get; set; }
    public int NumberOfTickets { get; set; }
    public bool IsComingSoon {get; set;}

    string MaskEmail(string input)
    {
        string pattern = @"([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})|((?i)\b((?:https?://|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'\""\.,<>?������]))\b)";
        return Regex.Replace(input, pattern, m => new string('*', m.Length));
    }

    string MaskPhone(string input)
    {
        string pattern = @"(\(?\d{3}\)?-? *\d{3}-? *-?\d{4})";
        return Regex.Replace(input, pattern, m => new string('*', m.Length));
    }

    public List<PriceTypeModel> PriceTypeModels { get; set; } = new List<PriceTypeModel>()
    {
        new PriceTypeModel
        {
            Description = "Pay To Reserve",
            PriceType = (int)Framework.Enums.Enums.PriceType.PayToReserve
        },
        new PriceTypeModel
        {
            Description = "Reserve Only",
            PriceType = (int)Framework.Enums.Enums.PriceType.ReserveOnly
        }
    };

    public OteSchedule? Schedule { get; set; }

    public class OteSchedule 
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IsReservedSeating { get; set; } = false;
    }
}

public class PriceTypeModel
{
    public string Description { get; set; }
    public int PriceType { get; set; }
}