using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Schedule.Request
{
    public class UpdateScheduleArgs
    {
        [Required]
        public int Id { get; set; }
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
        [Required]
        public bool isSetSession { get; set; } = false;
        [Required]
        public string SessionName { get; set; }
        [Required]
        public int Order {get; set;}
    }
}
