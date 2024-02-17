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
}