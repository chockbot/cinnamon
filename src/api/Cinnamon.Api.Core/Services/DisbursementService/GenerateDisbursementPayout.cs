using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Cinnamon.Api.Core.Services.Disbursement.Interactors;
using Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.Disbursement;

public class GenerateDisbursementPayout : IGenerateDisbursementPayout
{
    public AppResult<GenerateDisbursementPayoutResult> Execute(GenerateDisbursementPayoutArgs args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<GenerateDisbursementPayoutResult>> ExecuteAsync(GenerateDisbursementPayoutArgs args)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            return AppResult<GenerateDisbursementPayoutResult>.CreateFailed(ex, "An error occured when generating disbursement payout.");
        }
    }
}