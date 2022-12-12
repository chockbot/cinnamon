using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Api.Data.Models.Schedule.Request
{
    public class CreateScheduleArgs
    {
        [Required]
        public int ActivityId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DateTime { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string UnitPrice { get; set; } = "PHP";
        [Required]
        public int PerUnit1 { get; set; } = 1;
        [Required]
        public string PriceUnit1 { get; set; } = "Head";
        [Required]
        public int PerUnit2 { get; set; } = 1;
        [Required]
        public string PriceUnit2 { get; set; } = "Session";
    }
}
