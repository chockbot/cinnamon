using Cinnamon.Core;
using Cinnamon.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data;

public class PurchaseOrder : BaseDbSet<PurchaseOrderModel>, IPurchaseOrder
{
    public PurchaseOrder(DataStoreDbContext dbContext) : base(dbContext) { }
    protected override DbSet<PurchaseOrderModel> Table => mDbContext.PurchaseOrders;
}