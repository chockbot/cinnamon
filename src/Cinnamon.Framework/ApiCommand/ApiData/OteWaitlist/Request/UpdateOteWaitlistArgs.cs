using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteWaitlist.Request;

public class UpdateOteWaitlistArgs
{
    [Required]
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ProviderId { get; set; }
    public string CustomerName { get; set; }
    public int ActivityId { get; set; }
    public int ScheduleId { get; set; }
    public string Payload { get; set; }
    public int Status { get; set; }
}
