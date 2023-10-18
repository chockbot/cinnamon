using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class AddTicketSoldArgs
{
    [Required]
    public IEnumerable<AddTicketSold> TicketSolds {get; set;}

    public class AddTicketSold 
    {
        [Required]
        public int Id {get; set;}

        [Required]
        public int TicketSold {get; set;}
    }
}