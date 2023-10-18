using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteTicketEntity : GenericEntity<OteTicket>, IOteTicket 
{
    private readonly ApplicationContext applicationContext;

    public OteTicketEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<Entities.OteTicket>>> GetByActivityId(int activityId, int? count, int? skip, bool includeCustomer = false, bool includeImageData = false)
    {
        try
        {
            int limitCount = count.HasValue ? count.Value : int.MaxValue;
            int skipCount = skip.HasValue ? skip.Value : 0;

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

            var result = await query.Skip(skipCount).Take(limitCount).ToListAsync();
            return AppResult<IEnumerable<Entities.OteTicket>>.CreateSucceeded(result, "Successfully get tickets by activity id.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<Entities.OteTicket>>.CreateFailed(ex, "An error occured when getting tickets.");
        }
    }

    public async Task<AppResult<IEnumerable<Entities.OteTicket>>> GetByPurchaseOrderId(int purchaseOrderId, bool includeCustomer = false, bool includeImageData = false)
    {
        try
        {
            var query = applicationContext.OteTickets.Where(t => t.PurchaseOrderId == purchaseOrderId);

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

    public async Task<AppResult<IEnumerable<OteScheduleDTO>>> GetTicketDetails(int activityId)
    {
        try
        {
            string query = "SELECT a.\"Id\", a.\"ActivityId\", a.\"From\", a.\"To\", a.\"Recurrences\"," +
                "\r\nb.\"Name\", b.\"Description\",b.\"MaxSlots\",\r\n" +
                "(SELECT COUNT(*) FROM public.\"OteTickets\" WHERE \"OteSchedulePricingId\" = b.\"Id\") AS Sold\r\n" +
                "FROM public.\"OteSchedules\" as a\r\n" +
                "JOIN public.\"OteSchedulePricings\" as b ON b.\"OteScheduleId\" = a.\"Id\"\r\nWHERE a.\"ActivityId\" = "+ activityId + ";";

            IList<OteScheduleDTO> listResult = new List<OteScheduleDTO>();
            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);
                        //Get Activity
                        listResult = dt.AsEnumerable().Select(item => new OteScheduleDTO
                        {
                            ActivityId  = Convert.ToInt32(item["ActivityId"]),
                            From        = item["From"] != DBNull.Value ? Convert.ToDateTime(item["From"]) : DateTime.MinValue,
                            To          = item["To"] != DBNull.Value ? Convert.ToDateTime(item["To"]) : DateTime.MinValue,
                            Recurrences = item["Recurrences"].ToString() ?? string.Empty,
                            OteSchedulePricingDTO = new OteSchedulePricingDTO()
                            {
                                Name        = item["Name"].ToString() ?? string.Empty,
                                Description = item["Description"].ToString() ?? string.Empty,
                                MaxSlots    = Convert.ToInt32(item["MaxSlots"]),
                                Sold        = Convert.ToInt32(item["Sold"])
                            }
                        }).ToList();
                    }
                }
            }
            return AppResult<IEnumerable<OteScheduleDTO>>.CreateSucceeded(listResult, "Successfully get ticket details");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteScheduleDTO>>.CreateFailed(ex, "An error occured when trying to get ticket details");
        }
    }
}