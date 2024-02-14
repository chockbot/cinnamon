using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DisbursementBulkLogEntity : GenericEntity<DisbursementBulkLog>, IDisbursementBulkLog 
{
    private readonly ApplicationContext applicationContext;

    public DisbursementBulkLogEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}