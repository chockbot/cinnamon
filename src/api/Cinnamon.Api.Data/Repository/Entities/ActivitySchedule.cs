using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Api.Data.Repository.Entities;

public class ActivitySchedule : BaseEntity 
{
    public int ActivityId {get; set;}
    public string Name {get; set;}
    public string DateTime {get; set;}
    public decimal Price {get; set;}
    public string UnitPrice {get; set;} = "PHP";
    public int PerUnit1 { get; set; } = 1;
    public string PriceUnit1 { get; set; } = "Head";
    public int PerUnit2 { get; set; } = 1;
    public string PriceUnit2 { get; set; } = "Session";
    public int Order {get; set;}
    [DefaultValue(true)]
    public bool IsActiveSchedule { get; set; } = true;
    public virtual Activity Activity {get; set;}
}