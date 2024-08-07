using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class TopBookedCustomersArgs
{
    [Required]
    public int ActivityId {get; set;}

    // date format must yyyyMMddHHmmss
    [Required]
    public string BookedDate {get; set;}
    public int TotalParticipants {get; set;}
}
