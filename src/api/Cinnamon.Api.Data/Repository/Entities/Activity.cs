using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Api.Data.Repository.Entities;

public class Activity : BaseEntity
{
    public int ExperienceTypeId {get; set;}
    public string Title {get; set;}
    public string Subtitle {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public string Price {get; set;} = string.Empty;
    public string ScheduleIndicator {get; set;} = string.Empty;
    public string MapDetails {get; set;} = string.Empty;
    public string Guarantee {get; set;} = string.Empty;
    public string Remarks {get; set;} = string.Empty;
    public bool IsPublished { get; set;}
    public int? ExperienceCategoryId {get; set;}
    public int? SubCategoryId {get; set;}
    public string Handler {get; set;}
    public bool IsNew { get; set; }
    public int PurchaseOrderCount { get; set; }
    public bool IsDeactivated { get; set; }
    public string Guid { get; set; }
    public int Status { get; set; }
    public int ExperienceCreationTypeId { get; set; }
    public bool IsComingSoon {get; set;}

    public virtual ActivityAddress Address {get; set;}
    public virtual ActivityDescription ActivityDescription {get; set;}
    public virtual SearchTags SearchTag {get; set;}
    public virtual IList<ActivitySchedule> Schedules {get; set;}
    public virtual IList<ActivityImage> Images {get; set;}
    public virtual ExperienceType ExperienceType {get; set;}
    public virtual ExperienceCategory ExperienceCategory {get; set;}
    public virtual SubCategory SubCategory {get; set;}
    public virtual ExperienceCreationType ExperienceCreationType {get; set; }
    [ForeignKey("CreatedBy")]
    public virtual Customer Customer {get; set;}
    public virtual IList<Student> Students {get; set; }
    public virtual IList<Reviews> Reviews {get; set; }
    public virtual IList<OteTicket> Tickets { get; set; }
    public virtual OteSchedule OteSchedule { get; set; }
}