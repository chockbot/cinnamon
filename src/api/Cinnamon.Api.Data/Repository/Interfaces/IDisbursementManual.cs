using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IDisbursementManual : IGenericEntity<DisbursementManual> {
    Task<AppResult<DisbursementManual>> CreateManualDisbursement(DisbursementManual disbursementManual);
}