using System.Data;
using System.Data.Entity.Core.EntityClient;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PurchaseOrder;
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

    public async Task<AppResult<IEnumerable<PurchaseOrder>>> UpdatePurchaseOrdersByStatus(IEnumerable<PurchaseOrder> purchaseOrders)
    {
        try
        {
            foreach(var order in purchaseOrders)
            {
                var entity = await applicationContext.PurchaseOrders.FindAsync(order.Id);
                if(entity != null)
                {
                    entity.Status = order.Status;
                }
            }

            await applicationContext.SaveChangesAsync();

            return AppResult<IEnumerable<PurchaseOrder>>.CreateSucceeded(purchaseOrders, "Successfully update purchase orders");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<PurchaseOrder>>.CreateFailed(ex, "An error occured when trying to update purchase orders");
        }
    }

    public async Task<AppResult<IEnumerable<InclusivePurchaseOrderDTO>>> GetInclusiveTransaction(string? name, DateTime? purchaseDate, string? email, int? status)
    {
        try
        {
            string queryFilters = string.Empty;

            if(!string.IsNullOrEmpty(name))
            {
                queryFilters += "and (cc.\"FirstName\" like '%'@name'%' or cc.\"LastName\" like '%'@name'%') ";
            }

            if(purchaseDate.HasValue)
            {
                queryFilters += "and cast(po.\"CreatedOn\" as date) = @date ";
            }

            if(!string.IsNullOrEmpty(email))
            {
                queryFilters += "and cc.\"Email\" = @email ";
            }

            if(status.HasValue)
            {
                queryFilters += "and po.\"Status\" = @status ";
            }
            
            string query = "select po.\"Id\" as \"POId\", po.\"CreatedOn\", po.\"UnitCount\", po.\"UnitPrice\", " +
                                "po.\"UnitCount\" * po.\"UnitPrice\" as \"Total\", po.\"ConvinienceFee\", " +
                                "po.\"CreditAmount\", (po.\"UnitCount\" * po.\"UnitPrice\") - po.\"ConvinienceFee\" as \"OverallTotal\", " +
                                "po.\"Status\", cc.\"FirstName\", cc.\"LastName\", cc.\"Email\" " +
                            "from public.\"PurchaseOrders\" po " +
                            "join public.\"Activities\" ac " +
                                "on po.\"ActivityId\" = ac.\"Id\" " +
                            "join public.\"Customers\" cc " +
                                "on ac.\"CreatedBy\" = cc.\"Id\" " +
                            "where po.\"IsInclusivePayment\" = true " + queryFilters;

            IList<InclusivePurchaseOrderDTO> listResult = new List<InclusivePurchaseOrderDTO>();

            using(var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = query;

                if(!string.IsNullOrEmpty(name))
                {
                    var parameterName = new EntityParameter();
                    parameterName.ParameterName = "name";
                    parameterName.Value = name;
                    command.Parameters.Add(parameterName);
                }

                if(purchaseDate.HasValue)
                {
                    var parameterDate = new EntityParameter();
                    parameterDate.ParameterName = "date";
                    parameterDate.Value = purchaseDate?.ToString("YYYY-MM-dd");
                    command.Parameters.Add(parameterDate);
                }

                if(!string.IsNullOrEmpty(email))
                {
                    var parameterEmail = new EntityParameter();
                    parameterEmail.ParameterName = "email";
                    parameterEmail.Value = email;
                    command.Parameters.Add(parameterEmail);
                }

                if(status.HasValue) 
                {
                    var parameterStatus = new EntityParameter();
                    parameterStatus.ParameterName = "status";
                    parameterStatus.Value = status;
                    command.Parameters.Add(parameterStatus);
                }

                applicationContext.Database.OpenConnection();
                
                using(var dr = await command.ExecuteReaderAsync())
                {
                    if(dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        listResult = dt.AsEnumerable().Select(item => new InclusivePurchaseOrderDTO {
                            ConvinienceFee = Convert.ToDecimal(item["ConvinienceFee"]),
                            CreditAmount = Convert.ToDecimal(item["CreditAmount"]),
                            OverallTotal = Convert.ToDecimal(item["OverallTotal"]),
                            Provider = new InclusivePurchaseOrderDTO.ProviderDTO {
                                Email = item["Email"].ToString() ?? string.Empty,
                                FirstName = item["FirstName"].ToString() ?? string.Empty,
                                LastName = item["LastName"].ToString() ?? string.Empty
                            },
                            PurchaseDate = Convert.ToDateTime(item["CreatedOn"]),
                            PurchaseOrderId = Convert.ToInt32(item["POId"]),
                            Status = Convert.ToInt32(item["Status"]),
                            Total = Convert.ToDecimal(item["Total"]),
                            UnitCount = Convert.ToInt32(item["UnitCount"]),
                            UnitPrice = Convert.ToDecimal(item["UnitPrice"])
                        }).ToList();
                    }
                }

            }

            return AppResult<IEnumerable<InclusivePurchaseOrderDTO>>.CreateSucceeded(listResult, "Successfuly get list of transactions");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<InclusivePurchaseOrderDTO>>.CreateFailed(ex, "An error occured when getting inclusive transactions");
        }
    }
}