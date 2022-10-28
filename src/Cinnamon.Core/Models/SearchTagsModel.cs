using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Core.Models
{
    public class SearchTagsModel : BaseModel
    {
        public int Id { get; set; }
        [ForeignKey("Activity")]
        public int ActivityId { get; set; }
        public string? SearchTag1 { get; set; } = null;
        public string? SearchTag2 { get; set; } = null;
        public string? SearchTag3 { get; set; } = null;
        public string? SearchTag4 { get; set; } = null; 
        public string? SearchTag5 { get; set; } = null;
        public virtual ActivityModel Activity { get; set; }
    }
}
