using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Schedule.Request;

public class UpdateManySchedulesArgs
{
    [Required]
    public IEnumerable<UpdateSchedule> Schedules {get; set;}

    public class UpdateSchedule 
    {
        [Required]
        public int Id {get; set;}
        [Required]
        public int ActivityId {get; set;}
        public string? Name { get; set; }
        public string? DateTime { get; set; }
        public decimal? Price { get; set; }
        public string? UnitPrice { get; set; } = "PHP";
        public int? PerUnit1 { get; set; } = 1;
        public string? PriceUnit1 { get; set; } = "Head";
        public int? PerUnit2 { get; set; } = 1;
        public string? PriceUnit2 { get; set; } = "Session";
        public int? Order {get; set;}
        public bool? IsActiveSchedule { get; set; }
        public bool? IsSetSession { get; set; } = false;
        public string? SessionName { get; set; }
        public int? HasExpiration { get; set; } = 0;
        public DateTime StartDate { get; set; }
    }    
}