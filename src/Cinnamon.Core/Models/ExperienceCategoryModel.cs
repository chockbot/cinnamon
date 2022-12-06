using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Core.Models
{
    public class ExperienceCategoryModel:BaseModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string IconPath { get; set; }

        public virtual IList<SubCategoryModel> SubCategory { get; set; }
    }
}
