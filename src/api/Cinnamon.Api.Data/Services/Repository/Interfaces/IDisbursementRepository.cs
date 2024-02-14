using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Disbursement;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IDisbursementRepository
{
    Task<AppResult<IEnumerable<DisbursementDTO>>> CreateDisbursements(IEnumerable<DisbursementDTO> disbursements, 
        IEnumerable<int> studentIds, IEnumerable<int> purchaseOrderIds);
    
    Task<AppResult<DisbursementBulkDTO>> CreateDisbursementBulk(DisbursementBulkDTO disbursementBulkDTO);
    
    Task<AppResult<IEnumerable<DisbursementDTO>>> GetDisbursements(string? status, int count, int skip);

    Task<AppResult<DisbursementBulkDTO>> UpdateDisbursementBulkStatus(int disbursementBulkId, 
        string disbursementBulkStatus, string disbursementStatus, string remarks);
    
    Task<AppResult<DisbursementBulkLogDTO>> CreateDisbursementBulkLog(DisbursementBulkLogDTO disbursementBulkLog);

    Task<AppResult<DisbursementBulkDTO>> GetDisbursementBulk(int id);

    Task<AppResult<IEnumerable<DisbursementInformationDTO>>> GetDisbursementsInformation(string filterBy, string filterValue);

    Task<AppResult<IEnumerable<DisbursementDetailDTO>>> GetDisbursementDetails(int disbusementId);
}