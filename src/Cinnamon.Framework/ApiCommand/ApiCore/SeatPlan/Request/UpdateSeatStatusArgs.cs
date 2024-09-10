using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.SeatPlan.Request;

public class UpdateSeatStatusArgs
{
    public int ActivityId { get; set; }
    public DateTime EventDate { get; set; }
    public string CategoryId { get; set; }
    public string RowId { get; set; }
    public string SeatId { get; set; }
    public bool Occupied { get; set; }
}
