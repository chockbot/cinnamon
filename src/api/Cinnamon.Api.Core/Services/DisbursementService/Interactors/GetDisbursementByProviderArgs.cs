using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.Disbursement.Interactors;

public class GetDisbursementByProviderArgs : IInteractor
{
    public int ProviderId { get; set; }
    public string FilterBy { get; set; }
    public string FilterValue { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}
