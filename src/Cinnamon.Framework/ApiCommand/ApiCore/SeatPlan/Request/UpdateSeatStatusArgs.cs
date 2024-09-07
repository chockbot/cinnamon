using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.SeatPlan.Request;

public class UpdateSeatStatusArgs
{
    [Required]
    public int ActivityId { get; set; }
    [Required]
    public DateTime EventDate { get; set; }
    [Required]
    public string CategoryId { get; set; }
    [Required]
    public string RowId { get; set; }
    [Required]
    public string SeatId { get; set; }
    [Required]
    public bool Occupied { get; set; }
}
