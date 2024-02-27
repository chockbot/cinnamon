using Cinnamon.Api.Core.Services.Disbursement.Interactors;
using Cinnamon.Api.Core.Services.Disbursement.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.Disbursement.Handlers;

public interface IGenerateDisbursementPayout : IInteractorHandler<GenerateDisbursementPayoutArgs, AppResult<GenerateDisbursementPayoutResult>> 
{}