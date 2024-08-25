using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.SeatPlanService.Interactors;

public class GetTemplatesArgs : IInteractor
{
    public string? Name { get; set; }
    public int? Page { get; set; }
    public int? Limit { get; set; }
}