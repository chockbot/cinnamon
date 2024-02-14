using System.Data;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Disbursement;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DisbursementEntity : GenericEntity<Disbursement>, IDisbursement 
{
    private readonly ApplicationContext applicationContext;

    public DisbursementEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<Disbursement>>> CreateDisbursements(IEnumerable<Disbursement> disbursements, 
        IEnumerable<int> studentIds, IEnumerable<int> purchaseOrderIds)
    {
        try
        {
            applicationContext.Disbursements.AddRange(disbursements);
            
            foreach(var id in studentIds)
            {
                var student = await applicationContext.Students.FindAsync(id);
                if(student is not null)
                {
                    student.IsDisbursement = true;
                }
            }

            foreach(var id in purchaseOrderIds)
            {
                var purchaseOrder = await applicationContext.PurchaseOrders.FindAsync(id);
                if(purchaseOrder is not null)
                {
                    purchaseOrder.Status = 5;
                }
            }

            await applicationContext.SaveChangesAsync();

            return AppResult<IEnumerable<Disbursement>>.CreateSucceeded(disbursements, "Disbursement successfully created.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<Disbursement>>.CreateFailed(ex, "An error occured when creating disbursements");
        }
    }

    public async Task<AppResult<IEnumerable<DisbursementInformationDTO>>> GetDisbursementsInfo(string filterBy, string filterValue)
    {
        try
        {
            string whereClause = string.Empty;

            if(filterBy.Equals("email", StringComparison.CurrentCultureIgnoreCase))
            {
                whereClause += "where cc.\"Email\" = @email ";
            }
            if(filterBy.Equals("status", StringComparison.CurrentCultureIgnoreCase))
            {
                whereClause += "where ds.\"Status\" = @status";
            }

            string query = "select cc.\"Email\", cc.\"FirstName\", cc.\"LastName\", " +
                               "ds.\"Id\", ds.\"Label\", ds.\"Amount\", ds.\"Status\", ds.\"InclusivePayment\", " +
                               "ds.\"Remarks\" " +
                            "from public.\"Disbursements\" ds " +
                            "join public.\"PurchaseOrders\" po " +
                               "on po.\"Id\" = ds.\"PurchaseOrderId\" " +
                           "join public.\"Customers\" cc " +
                               "on cc.\"Id\" = ds.\"CustomerId\" " + whereClause;

            List<DisbursementInformationDTO> result = new();

            using(var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
				command.CommandType = System.Data.CommandType.Text;

                if(filterBy.Equals("email", StringComparison.CurrentCultureIgnoreCase))
                {
                    var parameterEmail = new NpgsqlParameter("email", filterValue);
                    command.Parameters.Add(parameterEmail);
                }
                if(filterBy.Equals("status", StringComparison.CurrentCultureIgnoreCase))
                {
                    var parameterEmail = new NpgsqlParameter("status", filterValue);
                    command.Parameters.Add(parameterEmail);
                }

				applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if(dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        result = dt.AsEnumerable().Select(item => new DisbursementInformationDTO {
                            Amount = Convert.ToDecimal(item["Amount"]),
                            Id = Convert.ToInt32(item["Id"]),
                            InclusivePayment = Convert.ToBoolean(item["InclusivePayment"]),
                            Label = item["Label"].ToString() ?? string.Empty,
                            ProviderEmail = item["Email"].ToString() ?? string.Empty,
                            ProviderFirstName = item["FirstName"].ToString() ?? string.Empty,
                            ProviderLastName = item["LastName"].ToString() ?? string.Empty,
                            Remarks = item["Remarks"].ToString() ?? string.Empty,
                            Status = item["Status"].ToString() ?? string.Empty
                        }).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<DisbursementInformationDTO>>.CreateSucceeded(result, "Successfully get disbursments info.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DisbursementInformationDTO>>.CreateFailed(ex, "An error occured when getting disbursements info.");
        }
    }
}