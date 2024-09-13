using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class CreateOteWaitlistArgs
{
    [Required]
    public int ProviderId { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public string CustomerName { get; set; }
    [Required]
    public int ActivityId { get; set; }
    [Required]
    public int ScheduleId { get; set; }
    [Required]
    public string Payload { get; set; }
    public int Status { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    public int OteDateId {get; set;}
}
