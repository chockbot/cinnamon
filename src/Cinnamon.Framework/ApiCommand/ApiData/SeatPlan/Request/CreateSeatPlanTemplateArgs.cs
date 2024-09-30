using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.SeatPlan.Request;

public class CreateSeatPlanTemplateArgs
{
    [Required]
    public int SeatPlanFormatterId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string ImageSrc { get; set; }

    [Required]
    public string Payload { get; set; }

    [Required]
    public bool Enabled { get; set; }

    [Required]
    public DateTime UploadedDate { get; set; }

    [Required]
    public int UploadedBy { get; set; }
}