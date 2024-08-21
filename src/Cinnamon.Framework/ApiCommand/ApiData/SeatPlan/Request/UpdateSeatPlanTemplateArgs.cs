using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.SeatPlan.Request;

public class UpdateSeatPlanTemplateArgs
{

    [Required]
    public int Id { get; set; }
    
    public int? SeatPlanFormatterId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? ImageSrc { get; set; }

    public string? Payload { get; set; }

    public bool? Enabled { get; set; }

    public DateTime? UploadedDate { get; set; }

    public int? UploadedBy { get; set; }
}
