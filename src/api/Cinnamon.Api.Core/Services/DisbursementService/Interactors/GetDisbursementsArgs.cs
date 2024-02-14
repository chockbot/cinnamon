using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.Disbursement.Interactors;

public class GetDisbursementsArgs : IInteractor 
{
    public string FilterBy {get; set;}
    public string FilterValue {get; set;}
}