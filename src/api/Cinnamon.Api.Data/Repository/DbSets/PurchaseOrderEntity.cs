using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class PurchaseOrderEntity : GenericEntity<PurchaseOrder>, IPurchaseOrder
{
    public PurchaseOrderEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}