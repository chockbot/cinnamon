using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;

public class ForceDisableActivitiesArgs 
{
    [Required]
    public IList<int> Ids {get; set;}
}