using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.SeatPlanService.Interactors;

public class UpdateSeatStatusArgs : IInteractor
{
    public int ActivityId { get; set; }
    public DateTime EventDate { get; set; }
    public string CategoryId { get; set; }
    public string RowId { get; set; }
    public string SeatId { get; set; }
    public bool Occupied { get; set; }
}