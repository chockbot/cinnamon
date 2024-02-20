using System.Linq.Expressions;
using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Disbursement;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.Disbursement;

public class DisbursementRepository : IDisbursementRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public DisbursementRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<IEnumerable<DisbursementDTO>>> CreateDisbursements(IEnumerable<DisbursementDTO> disbursements, 
        IEnumerable<int> studentIds, IEnumerable<int> purchaseOrderIds)
    {
        try
        {
            var entities = mapper.Map<IEnumerable<Entities.Disbursement>>(disbursements);

            var result = await dataStore.Disbursement.CreateDisbursements(entities, studentIds, purchaseOrderIds);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<DisbursementDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtos = mapper.Map<IEnumerable<DisbursementDTO>>(result.Result);
            return AppResult<IEnumerable<DisbursementDTO>>.CreateSucceeded(dtos, "Successfully create disbursements");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DisbursementDTO>>.CreateFailed(ex, "An error occured when creating disbursements.");
        }
    }

    public async Task<AppResult<DisbursementBulkDTO>> CreateDisbursementBulk(DisbursementBulkDTO disbursementBulkDTO)
    {
        try
        {
            var entity = mapper.Map<Entities.DisbursementBulk>(disbursementBulkDTO);

            var result = await dataStore.DisbursementBulk.CreateDisbursementBulk(entity);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<DisbursementBulkDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<DisbursementBulkDTO>(result.Result);
            return AppResult<DisbursementBulkDTO>.CreateSucceeded(dto, "Successfully create disbursement bulk.");
        }
        catch (Exception ex)
        {
            return AppResult<DisbursementBulkDTO>.CreateFailed(ex, "An error occured when creating disbursement bulk.");
        }
    }

    public async Task<AppResult<IEnumerable<DisbursementDTO>>> GetDisbursements(string? status, int count, int skip)
    {
        try
        {
            Expression<Func<Entities.Disbursement, bool>> filter =
                a => (!string.IsNullOrEmpty(status) ? a.Status == status : true);
            
            var result = await dataStore.Disbursement.FindAsync(filter, count, skip);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<DisbursementDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtos = mapper.Map<IEnumerable<DisbursementDTO>>(result.Result);
            return AppResult<IEnumerable<DisbursementDTO>>.CreateSucceeded(dtos, "Successfully get disbursements");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DisbursementDTO>>.CreateFailed(ex, "An error occured when getting disbursements.");
        }
    }

    public async Task<AppResult<DisbursementBulkDTO>> UpdateDisbursementBulkStatus(int disbursementBulkId, 
        string disbursementBulkStatus, string disbursementStatus, string remarks)
    {
        try
        {
            var result = await dataStore.DisbursementBulk.UpdateDisbursementBulkStatus(disbursementBulkId, 
                disbursementBulkStatus, disbursementStatus, remarks);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<DisbursementBulkDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<DisbursementBulkDTO>(result.Result);
            return AppResult<DisbursementBulkDTO>.CreateSucceeded(dto, "Successfully update disbursement bulk status.");
        }
        catch (Exception ex)
        {
            return AppResult<DisbursementBulkDTO>.CreateFailed(ex, "An error occured when updating disbursement bulk status.");
        }
    }

    public async Task<AppResult<DisbursementBulkLogDTO>> CreateDisbursementBulkLog(DisbursementBulkLogDTO disbursementBulkLog)
    {
        try
        {
            var entity = mapper.Map<Entities.DisbursementBulkLog>(disbursementBulkLog);
            
            var result = await dataStore.DisbursementBulkLog.Add(entity);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<DisbursementBulkLogDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<DisbursementBulkLogDTO>(result.Result);
            return AppResult<DisbursementBulkLogDTO>.CreateSucceeded(dto, "Successfully created disbursement bulk log.");
        }
        catch (Exception ex)
        {
            return AppResult<DisbursementBulkLogDTO>.CreateFailed(ex, "An error occured when creating disbursement bulk log.");
        }
    }

    public async Task<AppResult<DisbursementBulkDTO>> GetDisbursementBulk(int id)
    {
        try
        {
            var result = await dataStore.DisbursementBulk.FindFirstAsync(a => a.Id == id);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<DisbursementBulkDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<DisbursementBulkDTO>(result.Result);
            return AppResult<DisbursementBulkDTO>.CreateSucceeded(dto, "Successfully get disbursement bulk.");
        }
        catch (Exception ex)
        {
            return AppResult<DisbursementBulkDTO>.CreateFailed(ex, "An error occured when getting disbursement bulk.");
        }
    }

    public async Task<AppResult<IEnumerable<DisbursementInformationDTO>>> GetDisbursementsInformation(string filterBy, string filterValue)
    {
        try
        {
            var result = await dataStore.Disbursement.GetDisbursementsInfo(filterBy, filterValue);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<DisbursementInformationDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<IEnumerable<DisbursementInformationDTO>>.CreateSucceeded(result.Result, "Successfully get disbursement information");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DisbursementInformationDTO>>.CreateFailed(ex, "An error occured when getting disbursment information.");
        }
    }

    public async Task<AppResult<IEnumerable<DisbursementDetailDTO>>> GetDisbursementDetails(int disbusementId)
    {
        try
        {
            var result = await dataStore.DisbursementDetail.FindAsync(d => d.DisbursementId == disbusementId);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<DisbursementDetailDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dtos = mapper.Map<IEnumerable<DisbursementDetailDTO>>(result.Result);
            return AppResult<IEnumerable<DisbursementDetailDTO>>.CreateSucceeded(dtos, "Successfully get disbursement details.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DisbursementDetailDTO>>.CreateFailed(ex, "An error occured when getting disbursement details.");
        }
    }

    public async Task<AppResult<DisbursementDTO>> UpdateDisbursementStatus(int disbursementId, string status, string remarks)
    {
        try
        {
            var disbursementRes = await dataStore.Disbursement.FindFirstAsync(d => d.Id == disbursementId);
            if(!disbursementRes.Succeeded || disbursementRes.Result is null)
            {
                return AppResult<DisbursementDTO>.CreateFailed(new ApplicationException(disbursementRes.Message), disbursementRes.Message);
            }

            var disbursement = disbursementRes.Result;
            disbursement.Status = status;
            disbursement.Remarks = remarks;

            var updateDisbursementRes = await dataStore.Disbursement.Update(disbursement);
            if(!updateDisbursementRes.Succeeded || updateDisbursementRes.Result is null)
            {
                return AppResult<DisbursementDTO>.CreateFailed(new ApplicationException(updateDisbursementRes.Message), updateDisbursementRes.Message);
            }

            var dto = mapper.Map<DisbursementDTO>(updateDisbursementRes.Result);
            return AppResult<DisbursementDTO>.CreateSucceeded(dto, "Successfully update disbursement status.");
        }
        catch (Exception ex)
        {
            return AppResult<DisbursementDTO>.CreateFailed(ex, "An error occured when updating disbursement status.");
        }
    }

    public async Task<AppResult<DisbursementManualDTO>> CreateManualDisbursement(DisbursementManualDTO disbursementManual)
    {
        try
        {
            var entity = mapper.Map<Entities.DisbursementManual>(disbursementManual);
            
            var result = await dataStore.DisbursementManual.CreateManualDisbursement(entity);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<DisbursementManualDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var dto = mapper.Map<DisbursementManualDTO>(result.Result);
            return AppResult<DisbursementManualDTO>.CreateSucceeded(dto, "Successfully create manual disbursement.");
        }
        catch (Exception ex)
        {
            return AppResult<DisbursementManualDTO>.CreateFailed(ex, "An error occured when creating manual disbursement.");
        }
    }
}