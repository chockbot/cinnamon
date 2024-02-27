using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DisbursementDetailEntity : GenericEntity<DisbursementDetail>, IDisbursementDetail 
{
    private readonly ApplicationContext applicationContext;

    public DisbursementDetailEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}