using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class PayoutLogEntity : GenericEntity<PayoutLog>, IPayoutLog 
{
    private readonly ApplicationContext applicationContext;
    public PayoutLogEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<PayoutLog>>> GetPayoutByProvider(int? Id, DateTime? dateFrom)
    {
        try
        {
            string queryFilters = string.Empty;

            if (dateFrom.HasValue)
            {
                queryFilters += " AND \"CreatedOn\" between @dateFrom and CURRENT_DATE + 1";
            }

            string query = "SELECT \"Id\", \"PurchaseOrderId\", \"CustomerId\", \"Amount\", \"Status\", \"Remarks\", \"CreatedOn\", \"CreatedBy\", \"ChangedOn\", \"ChangedBy\", \"Payload\"\r\nFROM public.\"PayoutLogs\" " +
                "WHERE \"CustomerId\" = "+Id+" AND \"Status\" = 1" + queryFilters;
            
            IList<PayoutLog> listResult = new List<PayoutLog>();
            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                if (dateFrom.HasValue)
                {
                    var parameterDateFrom = new NpgsqlParameter("dateFrom", dateFrom);
                    command.Parameters.Add(parameterDateFrom);
                }

                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        listResult = dt.AsEnumerable().Select(item => new PayoutLog
                        {
                            Id              = Convert.ToInt32(item["Id"]),
                            PurchaseOrderId = Convert.ToInt32(item["PurchaseOrderId"]),
                            CustomerId      = Convert.ToInt32(item["CustomerId"]),
                            Amount          = Convert.ToDecimal(item["Amount"]),
                            Status          = Convert.ToInt32(item["Status"]),
                            CreatedOn       = Convert.ToDateTime(item["CreatedOn"]),
                        }).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<PayoutLog>>.CreateSucceeded(listResult, "Successfully get payout by date range");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PayoutLog>>.CreateFailed(ex, "An error occured when trying to get payout by date range");
        }
    }
}