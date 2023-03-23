using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class PurchaseOrderEntity : GenericEntity<PurchaseOrder>, IPurchaseOrder
{
    private readonly ApplicationContext applicationContext;

    public PurchaseOrderEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<PurchaseOrder>>> GetAllPurchaseOrdersNeedPayout()
    {
        try
        {
            string query = "select distinct a.* " +
                            "from public.\"PurchaseOrders\" a " +
                            "join public.\"OngoingActivities\" b " +
                                "on a.\"ActivityId\" = b.\"ActivityId\" " +
                            "join public.\"Students\" c " +
                                "on c.\"OngoingActivityId\" = b.\"Id\" " +
                            "where a.\"Status\" = 1 and c.\"SessionsAttended\" >= c.\"NumberOfSessions\" ";
            
            var result = await applicationContext.PurchaseOrders.FromSqlRaw(query).ToListAsync();
            if(result == null)
            {
                return AppResult<IEnumerable<PurchaseOrder>>.CreateFailed(new ApplicationException("An error occured when getting rows"), "An error occured when getting rows");
            }

            return AppResult<IEnumerable<PurchaseOrder>>.CreateSucceeded(result, "Successfully get purchase orders");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PurchaseOrder>>.CreateFailed(ex, "An error occured when trying to get purchase orders need to payout");
        }
    }
}