using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteReminderFlags.Request;

public class CreateReminderFlagArgs
{
    [Required]
    public int ActivityId {get; set;}

    [Required]
    public int OteDateId {get; set;}
}