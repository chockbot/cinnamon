using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;
public class AddActivityExpirationArgs
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int ScheduleId { get; set; }
    [Required]
    public string SessionName { get; set; }
    [Required]
    public DateTime ExpirationStartDate { get; set; }
    public DateTime ExpirationEndDate { get; set; }
}
