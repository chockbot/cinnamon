using System.Data;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DirectStudentSessionEntity : GenericEntity<DirectStudentSession>, IDirectStudentSession
{
    private readonly ApplicationContext applicationContext;

    public DirectStudentSessionEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<int>> OngoingStudentCount(int activityId)
    {
        try
        {
            var dateString = DateTime.Now.ToString("yyyy-MM-dd"); 

			var query = "select Count(st.\"Id\") \"Ongoing\" " +
						"from public.\"Activities\" ac " +
						"join public.\"DirectStudentSessions\" st " +
							"on ac.\"Id\" = st.\"ActivityId\" " +
						"where ac.\"Id\" = " + activityId + " and ( " +
												"(st.\"SessionsAttended\" < st.\"NumberOfSessions\" and st.\"ExpirationDateEnd\" = '-infinity') or " +
												"(st.\"ExpirationDateEnd\" != '-infinity' and Date(st.\"ExpirationDateEnd\") > Date('" + dateString + "') " +
													"and st.\"SessionsAttended\" < st.\"NumberOfSessions\") " +
											") ";
			int result = 0;

			using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = System.Data.CommandType.Text;
                
                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);
                        result = dt.AsEnumerable().Select(item => Convert.ToInt32(item["Ongoing"])).First();
                    }
                }
            }
            
            return AppResult<int>.CreateSucceeded(result, "Successfully get ongoing student count.");
        }
        catch (Exception ex)
        {
            return AppResult<int>.CreateFailed(ex, "An error occured when getting ongoing student count.");
        }
    }

    public async Task<AppResult<int>> CompletedStudentCount(int activityId)
    {
        try
        {
            var dateString = DateTime.Now.ToString("yyyy-MM-dd"); 

			var query = "select Count(st.\"Id\") \"Completed\" " +
						"from public.\"Activities\" ac " +
						"join public.\"DirectStudentSessions\" st " +
							"on ac.\"Id\" = st.\"ActivityId\" " +
						"where ac.\"Id\" = " + activityId + " and ( " +
													"(st.\"SessionsAttended\" >= st.\"NumberOfSessions\") or " +
													"(st.\"ExpirationDateEnd\" != '-infinity' and Date(st.\"ExpirationDateEnd\") <= Date('" + dateString + "') ) " +
												") ";
			int result = 0;

			using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = System.Data.CommandType.Text;
                
                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);
                        //Get Enrolled Student List
                        result = dt.AsEnumerable().Select(item => Convert.ToInt32(item["Completed"])).First();
                    }
                }
            }
            
            return AppResult<int>.CreateSucceeded(result, "Successfully get completed student count.");
        }
        catch (Exception ex)
        {
            return AppResult<int>.CreateFailed(ex, "An error occured when getting completed student count.");
        }
    }
}