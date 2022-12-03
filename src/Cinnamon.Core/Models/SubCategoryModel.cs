using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Core.Models
{
    public class SubCategoryModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CategoryModel")]
        public int CatergoryId { get; set; }
        public string SubCatergory { get; set; }

        public virtual ExperienceCategoryModel CategoryModel { get; set; }
    }
}
