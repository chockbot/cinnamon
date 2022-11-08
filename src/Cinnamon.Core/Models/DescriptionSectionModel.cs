using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Core.Models
{
    public class DescriptionSectionModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Activity")]
        public int ActivityId { get; set; }
        public string Description { get; set; }
        public string SpecificsYouWillProvide { get; set; }
        public string CustomerBringWithThem { get; set; }
        public string AdditionalRequirements { get; set; }
        public string ActivityLevel { get; set; }
        public string SkillLevel { get; set; }
        public string MinimumAge { get; set; }
        public bool CanAdultsJoin { get; set; } = true;
        public virtual ActivityModel Activity { get; set; }
    }
}
