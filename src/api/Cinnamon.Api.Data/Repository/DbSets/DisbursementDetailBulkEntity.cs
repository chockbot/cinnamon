using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DisbursementDetailBulkEntity : GenericEntity<DisbursementDetailBulk>, IDisbursementDetailBulk 
{
    private readonly ApplicationContext applicationContext;

    public DisbursementDetailBulkEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}