namespace Cinnamon.Api.Data.Repository.Entities;

public class SeatPlanFormatter : BaseEntity
{
    public string Name { get; set; }
    public string Handler { get; set; }
    public bool Enabled { get; set; }
}