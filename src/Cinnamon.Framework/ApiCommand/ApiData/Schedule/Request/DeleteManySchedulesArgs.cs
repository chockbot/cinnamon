using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Schedule.Request;

public class DeleteManySchedulesArgs
{
    [Required]
    public IEnumerable<int> ScheduleIds {get; set;}
}