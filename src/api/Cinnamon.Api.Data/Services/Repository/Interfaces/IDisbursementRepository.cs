using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Disbursement;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IDisbursementRepository
{
    Task<AppResult<IEnumerable<DisbursementDTO>>> CreateDisbursements(IEnumerable<DisbursementDTO> disbursements);
}