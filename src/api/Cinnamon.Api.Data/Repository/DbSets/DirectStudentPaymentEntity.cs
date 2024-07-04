using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DirectStudentPaymentEntity : GenericEntity<DirectStudentPayment>, IDirectStudentPayment
{
    private readonly ApplicationContext applicationContext;

    public DirectStudentPaymentEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }
    public async Task<AppResult<IEnumerable<DirectStudentPaymentDTO>>> GetStudentPaymentByProvider(int? ProviderId, DateTime? dateFrom)
    {
        try
        {
            string queryFilters = string.Empty;
            if (dateFrom.HasValue)
            {
                queryFilters += " AND c.\"CreatedOn\" between @dateFrom and CURRENT_DATE + 1";
            }

            string query = "SELECT a.\"Id\", a.\"ProviderId\", a.\"Name\",\r\n" +
                "b.\"ActivityId\", b.\"ScheduleId\",c.\"Amount\", c.\"CreatedOn\"\r\n" +
                "FROM public.\"DirectStudentInfos\" as a \r\n" +
                "JOIN public.\"DirectStudentSessions\" as b ON b.\"DirectStudentInfoId\" = a.\"Id\"\r\n" +
                "JOIN public.\"DirectStudentPayments\" as c ON c.\"DirectStudentSessionId\" = b.\"Id\"\r\n" +
                "WHERE a.\"ProviderId\" = " + ProviderId + " "+ queryFilters + ";";

            IList<DirectStudentPaymentDTO> listResult = new List<DirectStudentPaymentDTO>();
            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = System.Data.CommandType.Text;
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

                        listResult = dt.AsEnumerable().Select(item => new DirectStudentPaymentDTO
                        {
                            Amount = Convert.ToDecimal(item["Amount"]),
                            CreatedOn = item["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(item["CreatedOn"]) : DateTime.MinValue,
                        }).ToList();
                    }
                }
            }
            return AppResult<IEnumerable<DirectStudentPaymentDTO>>.CreateSucceeded(listResult, "Successfully get direct students payment");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DirectStudentPaymentDTO>>.CreateFailed(ex, "An error occured when trying to get direct students payment");
        }
    }
}