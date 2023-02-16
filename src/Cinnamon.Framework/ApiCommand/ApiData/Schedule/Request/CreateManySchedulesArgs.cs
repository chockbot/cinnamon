using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Schedule.Request;

public class CreateManySchedulesArgs
{
    [Required]
    public int ActivityId {get; set;}
    [Required]
    public IEnumerable<Schedule> Schedules {get; set;}

    public class Schedule 
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DateTime { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string UnitPrice { get; set; } = "PHP";
        public int PerUnit1 { get; set; } = 1;
        public string PriceUnit1 { get; set; } = "Head";
        public int PerUnit2 { get; set; } = 1;
        public string PriceUnit2 { get; set; } = "Session";
        public bool isSetSession { get; set; } = false;
        public string SessionName { get; set; } 
    }    
}