using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class UpdateScheduleArgs : IInteractor
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? DateTime { get; set; }
    public decimal? Price { get; set; }
    public string? UnitPrice { get; set; } = "PHP";
    public int? PerUnit1 { get; set; } = 1;
    public string? PriceUnit1 { get; set; } = "Head";
    public int? PerUnit2 { get; set; } = 1;
    public string? PriceUnit2 { get; set; } = "Session";
    public bool? IsSetSession { get; set; } = false;
    public string? SessionName { get; set; }
    public int? Order { get; set; }
    public bool? IsActiveSchedule { get; set; }
}