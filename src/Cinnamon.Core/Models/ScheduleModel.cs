using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Core
{
    public class ScheduleModel : BaseModel
    {
        [Key]
        public int ScheduleId { get; set; }
        [ForeignKey("Activity")]
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public string DateTime { get; set; }
        public decimal Price { get; set; } = 0;
        public string UnitPrice { get; set; } = "PHP";
        public int PerUnit1 { get; set; } = 1;
        public string PriceUnit1 { get; set; } = "Head";
        public int PerUnit2 { get; set; } = 1;
        public string PriceUnit2 { get; set; } = "Session";
        public virtual ActivityModel Activity { get; set; }
    }
}
