namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivitySchedule;

public class ActivityScheduleDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DateTime { get; set; }
    public decimal Price { get; set; }
    public string UnitPrice { get; set; }
    public int PerUnit1 { get; set; }
    public string PriceUnit1 { get; set; }
    public int PerUnit2 { get; set; }
    public string PriceUnit2 { get; set; }
    public bool IsSetSession { get; set; }
    public string SessionName { get; set; } 

}