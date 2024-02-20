using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.Disbursement.Interactors;

public class ManualDisbursementArgs : IInteractor 
{
    public int DisbursementId {get; set;}
    public string Remarks {get; set;}
}