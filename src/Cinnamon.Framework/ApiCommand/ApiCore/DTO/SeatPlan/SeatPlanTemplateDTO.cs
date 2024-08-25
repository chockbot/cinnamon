namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.SeatPlan;

public class SeatPlanTemplateDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string ImageSrc { get; set; }
    public string Payload { get; set; }
    public string UploadedBy { get; set; }
    public DateTime UploadedDate { get; set; }
    public bool Enabled { get; set; }
}
