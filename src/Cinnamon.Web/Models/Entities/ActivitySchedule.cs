namespace Cinnamon.Web.Models.Entities;

public class ActivitySchedule 
{
    public int Id {get; set;}
    public string Name {get; set;}
    public string DateTime {get; set;}
    public decimal Price {get; set;}
    public string UnitPrice {get; set;} = "PHP";
    public int PerUnit1 {get; set;}
    public string PriceUnit1 {get; set;} = "Head";
    public int PerUnit2 {get; set;}
    public string PriceUnit2 {get; set;} = "Session";
}