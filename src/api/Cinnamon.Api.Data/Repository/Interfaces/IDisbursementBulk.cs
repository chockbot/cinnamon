using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IDisbursementBulk : IGenericEntity<DisbursementBulk> {
    Task<AppResult<DisbursementBulk>> CreateDisbursementBulk(DisbursementBulk disbursementBulk);

    Task<AppResult<DisbursementBulk>> UpdateDisbursementBulkStatus(int disbursementBulkId, 
        string disbursementBulkStatus, string disbursementStatus, string remarks);
}