using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.Disbursement.Interactors;

public class GetDisbursementDetailsArgs : IInteractor 
{
    public int DisbursementId {get; set;}
}