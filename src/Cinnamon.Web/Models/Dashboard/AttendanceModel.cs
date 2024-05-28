using Cinnamon.Framework.Enums;
using Cinnamon.Web.Models.Entities;

namespace Cinnamon.Web.Models.Dashboard;

public class AttendanceModel 
{
    public List<Schedule> Schedules {get; set;} = new();
    public Dictionary<string,StudentAttendance> Attendances {get; set;} = new();
    public List<StudentAttendance> FilteredAttendance {get; set;} = new();
    public List<Activity> Activities { get; set;} = new();
    public bool IsShowErrorMessage {get; set;}
    public string ErrorMessage {get; set;}
    public bool IsCheckAllStudent {get; set;}
    public string Token {get; set;}
    // sorting fields
    public bool? IsSortNameAsc {get; set;}
    public bool? IsSortExperienceAsc {get; set;}
    public bool? IsSortPresentAsc {get; set;}

    public string Search {get; set;}

    public bool IsProcessingCheckAll {get; set;}

    public DateTime ServerDate {get; set;}

    public class Schedule 
    {
        public int ActivityId {get; set;}
        public string ActivityTitle {get; set;}
        public string ActivityDescription {get; set;}
        public int ScheduleId {get; set;}
        public string ScheduleTitle {get; set;}
        public string ScheduleDescription {get; set;}
        public bool IsSelected {get; set;}
        public bool IsActiveSchedule { get; set;}
        public bool IsSetSession { get; set; }
        public string SessionName { get; set; }
        public int HasExpiration { get; set; }
        public DateTime? StartDate { get; set; }
    }

    public class StudentAttendance 
    {
        public int StudentId {get; set;}
        public int ActivityId {get; set;}
        public int ScheduleId {get; set;}
        public string Name {get; set;}
        public bool IsPresent {get; set;}
        public string ActivityName {get; set;}
        public int NumberOfSessions {get; set;}
        public int SessionsAttended {get; set;}
        public DateTime ExpirationDateStart { get; set; }
        public DateTime ExpirationDateEnd { get; set; }
        public StudentType StudentType {get; set;} = StudentType.Cinnamon;
    }
    public class Activity
    {
        public bool IsSelected { get; set; }
        public int ActivityId { get; set; }
        public int ExperienceTypeId { get; set; }
        public int ExperienceCategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<ActivitySchedule> ActivitySchedules { get; set; } = new List<ActivitySchedule>();


    }
}

