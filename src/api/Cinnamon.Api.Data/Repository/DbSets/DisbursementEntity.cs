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

	public async Task<AppResult<IEnumerable<DisbursementDTO>>> GetDisbursementsByProvider(string payoutDateString, int? Id, string filterBy, 
		string filterValue, int? count, int? skip)
	{
		try
		{
			int limitCount = count.HasValue ? count.Value : int.MaxValue;
			int skipCount = skip.HasValue ? skip.Value : 0;

			string whereClause = string.Empty;
			if (Id.HasValue)
			{
				whereClause += "where ds.\"CustomerId\" = @Id";
			}
			if (filterBy.Equals("name", StringComparison.CurrentCultureIgnoreCase))
			{
				filterValue = "%" + filterValue + "%";
				whereClause += " AND (SELECT cc_sub.\"FirstName\" || ' ' || cc_sub.\"LastName\" FROM public.\"Customers\" cc_sub WHERE cc_sub.\"Id\" = po.\"CustomerId\") ILIKE @name ";
			}
			if (filterBy.Equals("status", StringComparison.CurrentCultureIgnoreCase))
			{
				whereClause += " AND ds.\"Status\" = @status";
			}
			
			string query = "with filtered as " +
							"( " +
								"SELECT ds.\"Id\", ds.\"CustomerId\" as ProviderId, cc.\"Email\", cc.\"FirstName\", cc.\"LastName\", ds.\"Label\", " +
									"ds.\"Amount\", ds.\"Status\", ds.\"InclusivePayment\",ds.\"Remarks\", po.\"Payload\", " +
									"case " +
										"when ds.\"Status\" = 'initiated' then '" + payoutDateString + "' " +
										"else ds.\"ChangedOn\" " +
									"end as \"PayoutDate\", " +
									"(cc.\"FirstName\" || ' ' || cc.\"LastName\") as CustomerName " +
								"FROM public.\"Disbursements\" ds " +
								"JOIN public.\"PurchaseOrders\" po " +
									"ON po.\"Id\" = ds.\"PurchaseOrderId\" " +
								"JOIN public.\"Customers\" cc " +
									"ON cc.\"Id\" = ds.\"CustomerId\" " + whereClause + " " +
							") " +
							"select * " +
							"from filtered " +
							"order by \"PayoutDate\" desc ";

			List<DisbursementDTO> result = new();

			using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
			{
				command.CommandText = query;
				command.CommandType = System.Data.CommandType.Text;
				if (Id.HasValue)
				{
					var parameterCustomerId = new NpgsqlParameter("Id", Id.Value);
					command.Parameters.Add(parameterCustomerId);
				}
				if (filterBy.Equals("name", StringComparison.CurrentCultureIgnoreCase))
				{
					var parameterEmail = new NpgsqlParameter("name", filterValue);
					command.Parameters.Add(parameterEmail);
				}
				if (filterBy.Equals("status", StringComparison.CurrentCultureIgnoreCase))
				{
					var parameterStatus = new NpgsqlParameter("status", filterValue);
					command.Parameters.Add(parameterStatus);
				}
				applicationContext.Database.OpenConnection();
				using (var dr = await command.ExecuteReaderAsync())
				{
					if (dr.HasRows)
					{
						var dt = new DataTable();
						dt.Load(dr);

						result = dt.AsEnumerable().Select(item => new DisbursementDTO
						{
							Amount            = Convert.ToDecimal(item["Amount"]),
							Id                = Convert.ToInt32(item["Id"]),
							InclusivePayment  = Convert.ToBoolean(item["InclusivePayment"]),
							Label             = item["Label"].ToString() ?? string.Empty,
							Remarks           = item["Remarks"].ToString() ?? string.Empty,
							Status            = item["Status"].ToString() ?? string.Empty,
							Payload			  = item["Payload"].ToString() ?? string.Empty,
							PayoutDate	      = item["PayoutDate"] != DBNull.Value ? Convert.ToDateTime(item["PayoutDate"]) : DateTime.MinValue,
							DisbursementInformation = new DisbursementInformationDTO
							{
								ProviderId			= Convert.ToInt32(item["ProviderId"]),
								ProviderEmail		= item["Email"].ToString() ?? string.Empty,
								ProviderFirstName	= item["FirstName"].ToString() ?? string.Empty,
								ProviderLastName	= item["LastName"].ToString() ?? string.Empty,
								CustomerName        = item["CustomerName"].ToString() ?? string.Empty,
							}
						}).Skip(skipCount).Take(limitCount).ToList();
					}
				}
			}

			return AppResult<IEnumerable<DisbursementDTO>>.CreateSucceeded(result, "Successfully get disbursements info.");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<DisbursementDTO>>.CreateFailed(ex, "An error occurred when getting disbursements info.");
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

			int disbursementId = 0;
			if(filterBy.Equals("disbursementId", StringComparison.CurrentCultureIgnoreCase))
			{
				int.TryParse(filterValue, out disbursementId);
				whereClause += "where ds.\"Id\" = @id";
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
					var parameterStatus = new NpgsqlParameter("status", filterValue);
					command.Parameters.Add(parameterStatus);
				}
				if(filterBy.Equals("disbursementId", StringComparison.CurrentCultureIgnoreCase))
				{
					var parameterId = new NpgsqlParameter("id", disbursementId);
					command.Parameters.Add(parameterId);
				}

				applicationContext.Database.OpenConnection();

				using (var dr = await command.ExecuteReaderAsync())
				{
					if(dr.HasRows)
					{
						var dt = new DataTable();
						dt.Load(dr);

						result = dt.AsEnumerable().Select(item => new DisbursementInformationDTO {
							Amount            = Convert.ToDecimal(item["Amount"]),
							Id                = Convert.ToInt32(item["Id"]),
							InclusivePayment  = Convert.ToBoolean(item["InclusivePayment"]),
							Label             = item["Label"].ToString() ?? string.Empty,
							ProviderEmail     = item["Email"].ToString() ?? string.Empty,
							ProviderFirstName = item["FirstName"].ToString() ?? string.Empty,
							ProviderLastName  = item["LastName"].ToString() ?? string.Empty,
							Remarks           = item["Remarks"].ToString() ?? string.Empty,
							Status            = item["Status"].ToString() ?? string.Empty
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