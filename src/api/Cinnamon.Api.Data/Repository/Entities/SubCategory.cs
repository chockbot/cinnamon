using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Api.Data.Repository.Entities;

public class SubCategory: BaseEntity
{
    public int CatergoryId { get; set; }
    public string SubCatergory { get; set; }
}

