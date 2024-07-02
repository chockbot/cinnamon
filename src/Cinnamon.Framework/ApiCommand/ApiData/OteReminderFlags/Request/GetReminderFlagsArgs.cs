using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteReminderFlags.Request;

public class GetReminderFlagsArgs
{
    public int? ActivityId {get; set;}
    public int? OteDateId {get; set;}
}