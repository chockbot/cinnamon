using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteTicketEntity : GenericEntity<OteTicket>, IOteTicket 
{
    private readonly ApplicationContext applicationContext;

    public OteTicketEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<Entities.OteTicket>>> GetByActivityId(int activityId, bool includeCustomer = false, bool includeImageData = false)
    {
        try
        {
            var query = applicationContext.OteTickets.Where(t => t.ActivityId == activityId);

            if(includeCustomer)
            {
                query = query.Include(t => t.Customer);
            }

            if(!includeImageData)
            {
                query = query.Select(t => new OteTicket {
                    ActivityId = t.ActivityId,
                    Amount = t.Amount,
                    CustomerId = t.CustomerId,
                    Id = t.Id,
                    OteScheduleId = t.OteScheduleId,
                    OteSchedulePricingId = t.OteSchedulePricingId,
                    PurchaseOrderId = t.PurchaseOrderId,
                    QRCode = t.QRCode,
                    Status = t.Status,
                    Title = t.Title,
                    Customer = t.Customer
                });
            }

            var result = await query.ToListAsync();
            return AppResult<IEnumerable<Entities.OteTicket>>.CreateSucceeded(result, "Successfully get tickets by activity id.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<Entities.OteTicket>>.CreateFailed(ex, "An error occured when getting tickets.");
        }
    }
}