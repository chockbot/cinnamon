using System.ComponentModel.DataAnnotations;
using Cinnamon.Framework.Enums;
using Microsoft.AspNetCore.Http;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class CreateActivityArgs 
{
    [Required]
    public int ExperienceTypeId {get; set;}
    [Required]
    public int ExperienceCategoryId {get; set;}
    [Required]
    public int SubCategoryId {get; set;}
    [Required]
    public string Title {get; set;}
    [Required]
    public string Description {get; set;}
    [Required]
    public string Price {get; set;}
    public string? ScheduleIndicator {get; set;}
    public string? Remarks {get; set;}
    [Required]
    public bool IsPublished {get; set;}
    public string? Address1 {get; set;}
    public string? Address2 {get; set;}
    public string? District {get; set;}
    private string _city;

    public string City
    {
        get { return _city ?? string.Empty; }
        set { _city = value; }
    }

    private string _subdivision;

    public string Subdivision
    {
        get { return _subdivision ?? string.Empty; }
        set { _subdivision = value; }
    }

    private string _region;

    public string Region
    {
        get { return _region ?? string.Empty; }
        set { _region = value; }
    }

    private string _barangay;

    public string Barangay
    {
        get { return _barangay ?? string.Empty; }
        set { _barangay = value; }
    }

    private string _postalCode;

    public string PostalCode
    {
        get { return _postalCode ?? string.Empty; }
        set { _postalCode = value; }
    }

    private string _pinnedLocation;

    public string PinnedLocation
    {
        get { return _pinnedLocation ?? string.Empty; }
        set { _pinnedLocation = value; }
    }

    private string _specificsYouWillProvide;

    public string SpecificsYouWillProvide
    {
        get { return _specificsYouWillProvide ?? string.Empty; }
        set { _specificsYouWillProvide = value; }
    }

    private string _customerBringWithThem;

    public string CustomerBringWithThem
    {
        get { return _customerBringWithThem ?? string.Empty; }
        set { _customerBringWithThem = value; }
    }

    private string _additionalRequirements;

    public string AdditionalRequirements
    {
        get { return _additionalRequirements ?? string.Empty; }
        set { _additionalRequirements = value; }
    }

    [Required]
    public string ActivityLevel {get; set;}
    [Required]
    public string SkillLevel {get; set;}
    [Required]
    public int MinimumAge {get; set;}
    [Required]
    public bool CanAdultsJoin {get; set;}
    [Required]
    public IEnumerable<string> SearchTags {get; set;}
    [Required]
    public IEnumerable<Schedule> ActivitySchedules {get; set;}

    public Enums.Enums.ActivityStatus Status { get; set; }
    public Enums.Enums.ExperienceCreationType ExperienceCreationType { get; set; }

    public class Schedule 
    {
        [Required]
        public string Name {get; set;}
        [Required]
        public string DateTime {get; set;}
        [Required]
        public decimal Price {get; set;}
        [Required]
        public string UnitPrice {get; set;}
        [Required]
        public int PerUnit1 {get; set;}
        [Required]
        public string PriceUnit1 {get; set;}
        [Required]
        public int PerUnit2 {get; set;}
        [Required]
        public string PriceUnit2 {get; set;}
        [Required]
        public int Order {get; set;}
        [Required]
        public bool IsActiveSchedule { get; set; }
        [Required]
        public bool IsSetSession { get; set; }

        private string _sessionName;

        [Required]
        public string SessionName 
        {
            get { return _sessionName ?? string.Empty; }
            set { _sessionName = value; }
        }
        [Required]
        public int HasExpiration { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        public Enums.Enums.ScheduleType ScheduleType { get; set; }
        public Enums.Enums.PriceType PriceType { get; set; }
        public IEnumerable<ActivityScheduleTime> ActivityScheduleTimes { get; set; }
    }

    public IFormFile? Image1 { get; set; }
    public IFormFile? Image2 { get; set; }
    public IFormFile? Image3 { get; set; }
    public class ActivityScheduleTime
    {
        public int ActivityScheduleId { get; set; }
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}