using System.ComponentModel.DataAnnotations.Schema;

namespace Cinnamon.Api.Data.Repository.Entities;

public class SearchTags : BaseEntity 
{
    public int ActivityId {get; set;}
    public string? SearchTag1 {get; set;}
    public string? SearchTag2 {get; set;}
    public string? SearchTag3 {get; set;}
    public string? SearchTag4 {get; set;}
    public string? SearchTag5 {get; set;}


    public virtual Activity Activity {get; set;}   
}