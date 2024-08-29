using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.SeatPlan.Request;

public class ChangeSeatPlanStatusArgs 
{
    [Required]
    public int Id { get; set; }

    [Required]
    public bool Enabled { get; set; }
}