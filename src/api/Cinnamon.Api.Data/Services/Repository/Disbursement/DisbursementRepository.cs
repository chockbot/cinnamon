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
}