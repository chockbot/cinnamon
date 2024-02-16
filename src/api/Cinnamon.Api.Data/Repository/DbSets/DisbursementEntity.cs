using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DisbursementEntity : GenericEntity<Disbursement>, IDisbursement 
{
    private readonly ApplicationContext applicationContext;

    public DisbursementEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
}