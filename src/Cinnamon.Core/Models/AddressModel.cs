using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Core.Models
{
    public class AddressModel : BaseModel
    {
        [Key]
        public int AddressId { get; set; }
        [ForeignKey("Activity")]
        public int ActivityId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public virtual ActivityModel Activity { get; set; }
    }
}
