using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Description;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Disbursement;

public class DisbursementRepository 
{
    private readonly IDataStore dataStore;

    public DisbursementRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    
}