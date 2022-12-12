using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Api.Data.Repository.Entities;

public class SubCategory: BaseEntity
{
    public int CategoryId { get; set; }
    public string Subcategory { get; set; }
}

