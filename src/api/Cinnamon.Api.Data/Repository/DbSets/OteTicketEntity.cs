using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
using System;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class OteTicketEntity : GenericEntity<OteTicket>, IOteTicket
{
    private readonly ApplicationContext applicationContext;

    public OteTicketEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<Entities.OteTicket>>> GetByActivityId(int activityId, int dateId, string searchValue, int searchBy ,int? count, int? skip, 
        bool includeCustomer = false, bool includeImageData = false)
    {
        try
        {
            int limitCount = count.HasValue ? count.Value : int.MaxValue;
            int skipCount = skip.HasValue ? skip.Value : 0;

            var query = applicationContext.OteTickets.Where(t => t.ActivityId == activityId && t.OteDateId == dateId);
            if (!string.IsNullOrEmpty(searchValue))
            {
                switch (searchBy)
                {
                    case 1:
                        int ticketId = int.Parse(searchValue);
                        query = query.Where(t => t.Id == ticketId);
                        break;
                    case 2:
                        query = query.Where(t => EF.Functions.Like((t.Customer.FirstName + " " + t.Customer.LastName).ToLower(), $"%{searchValue.ToLower()}%"));
                        break;
                    case 3:
                        query = query.Where(t => EF.Functions.Like((t.Customer.Email).ToLower(), $"%{searchValue.ToLower()}%"));
                        break;
                    default:
                        break;
                }

            }
            if (includeCustomer)
            {
                query = query.Include(t => t.Customer);
            }

            if (!includeImageData)
            {
                query = query.Select(t => new OteTicket
                {
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

            if (includeCustomer)
            {
                query = query.Include(t => t.Customer);
            }

            if (!includeImageData)
            {
                query = query.Select(t => new OteTicket
                {
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

    public async Task<AppResult<IEnumerable<OteScheduleDTO>>> GetTicketDetails(int activityId, int dateId)
    {
        try
        {
            string query = "SELECT a.\"Id\",  a.\"ActivityId\", a.\"From\",  a.\"To\", a.\"Recurrences\", b.\"Name\", b.\"Description\", b.\"MaxSlots\", b.\"TicketSold\" ,b.\"Price\", b.\"Id\" as PricingId, b.\"RequiredApproval\" \r\n" +
                "FROM public.\"OteSchedules\" AS a\r\n" +
                "JOIN public.\"OteSchedulePricings\" AS b ON b.\"OteScheduleId\" = a.\"Id\"\r\n" +
                "WHERE a.\"ActivityId\" = " + activityId + " and b.\"OteDateId\" = " + dateId + ";";

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
                            ActivityId = Convert.ToInt32(item["ActivityId"]),
                            From = item["From"] != DBNull.Value ? Convert.ToDateTime(item["From"]) : DateTime.MinValue,
                            To = item["To"] != DBNull.Value ? Convert.ToDateTime(item["To"]) : DateTime.MinValue,
                            Recurrences = item["Recurrences"].ToString() ?? string.Empty,
                            OteSchedulePricingDTO = new OteSchedulePricingDTO()
                            {
                                Id                    = Convert.ToInt32(item["Id"]),
                                Name                  = item["Name"].ToString() ?? string.Empty,
                                Description           = item["Description"].ToString() ?? string.Empty,
                                MaxSlots              = Convert.ToInt32(item["MaxSlots"]),
                                Sold                  = Convert.ToInt32(item["TicketSold"]),
                                Price                 = Convert.ToDecimal(item["Price"]),
                                OteSchedulePricingsId = Convert.ToInt32(item["PricingId"]),
                                RequiredApproval      = Convert.ToBoolean(item["RequiredApproval"]),
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