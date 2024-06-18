using System.Data;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

    public async Task<AppResult<IEnumerable<DirectStudentSession>>> StudentSessions(int? studentId, 
        bool? ongoingSessions = false, bool? completedSessions = false)
    {
        try
        {
            var whereClause = "where 1=1 ";

            if(studentId.HasValue)
            {
                whereClause += "ds.\"DirectStudentInfoId\" = @studentId ";
            }

            if(ongoingSessions.HasValue && ongoingSessions.Value)
            {
                whereClause = "and ( " +
										"(ds.\"SessionsAttended\" < ds.\"NumberOfSessions\" and ds.\"ExpirationDateEnd\" = '-infinity') or " +
										"(ds.\"ExpirationDateEnd\" != '-infinity' and Date(ds.\"ExpirationDateEnd\") > Date(current_timestamp) " +
											"and ds.\"SessionsAttended\" < ds.\"NumberOfSessions\") " +
                                   ") ";
            }

            // override where clause
            if(completedSessions.HasValue && completedSessions.Value)
            {
                whereClause = "and ( " +
                                     "(ds.\"SessionsAttended\" >= ds.\"NumberOfSessions\") or " +
						             "(ds.\"ExpirationDateEnd\" != '-infinity' and Date(ds.\"ExpirationDateEnd\") <= Date(current_timestamp) ) " +
                                   ") ";
            }

            var query = "select ds.\"Id\" \"SessionId\", ds.\"DirectStudentInfoId\", ds.\"ActivityId\", ds.\"ScheduleId\", " +
                            "ds.\"Name\", ds.\"StudentNo\", ds.\"NumberOfSessions\", ds.\"SessionsAttended\", ds.\"Remarks\", " +
                            "ds.\"Status\", ds.\"ExpirationDateStart\", ds.\"ExpirationDateEnd\", dp.\"Id\" \"PaymentId\", " +
                            "dp.\"Amount\", dp.\"PaymentDate\" " +
                        "from public.\"DirectStudentSessions\" ds " +
                        "join public.\"DirectStudentPayments\" dp " +
                            "on ds.\"Id\" = dp.\"DirectStudentSessionId\" " + whereClause;

            var result = new List<DirectStudentSession>();

            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = System.Data.CommandType.Text;
                if(studentId.HasValue)
                {
                    command.Parameters.Add(new NpgsqlParameter("studentId", studentId));
                }
                
                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        result.AddRange(dt.AsEnumerable().Select(item => new DirectStudentSession {
                            ActivityId = Convert.ToInt32(item["ActivityId"]),
                            DirectStudentInfoId = Convert.ToInt32(item["DirectStudentInfoId"]),
                            ExpirationDateEnd = Convert.ToDateTime(item["ExpirationDateEnd"]),
                            ExpirationDateStart = Convert.ToDateTime(item["ExpirationDateStart"]),
                            Id = Convert.ToInt32(item["SessionId"]),
                            Name = item["Name"].ToString() ?? string.Empty,
                            NumberOfSessions = Convert.ToInt32(item["NumberOfSessions"]),
                            Remarks = item["Remarks"].ToString() ?? string.Empty,
                            ScheduleId = Convert.ToInt32(item["ScheduleId"]),
                            SessionsAttended = Convert.ToInt32(item["SessionsAttended"]),
                            Status = item["Status"].ToString() ?? string.Empty,
                            StudentNo = item["StudentNo"].ToString() ?? string.Empty,
                            DirectStudentPayment = new DirectStudentPayment {
                                Amount = Convert.ToDecimal(item["Amount"]),
                                Id = Convert.ToInt32(item["PaymentId"]),
                                PaymentDate = Convert.ToDateTime(item["PaymentDate"])
                            }
                        }));
                    }
                }
            }

            return AppResult<IEnumerable<DirectStudentSession>>.CreateSucceeded(result, "Successfully get direct student sessions.");

        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DirectStudentSession>>.CreateFailed(ex, "An error occured when getting student sessesions.");
        }
    }

    public async Task<AppResult<IEnumerable<ExpiredStudentDTO>>> ExpiringStudents()
    {
        try
		{
			// expired in two days
			var dateString = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");

			string query = "with uniqueRows as " +
                           "( " +
                               "select distinct ct.\"Id\" \"customerId\", st.\"ExpirationDateStart\" \"DateStart\", st.\"ExpirationDateEnd\" \"DateEnd\", " +
                                           "ct.\"Name\", ct.\"Email\", " +
                                           "ac.\"Title\", ac.\"Handler\", ss.\"Price\", ac.\"Price\" \"APrice\", " +
                                           "ac.\"Id\", ac.\"ExperienceTypeId\", ss.\"Id\" \"scheduleId\" " +
                               "from public.\"DirectStudentSessions\" st " +
                               "join public.\"DirectStudentInfos\" ct " +
                                   "on ct.\"Id\" = st.\"DirectStudentInfoId\" " +
                               "join public.\"Activities\" ac " +
                                   "on ac.\"Id\" = st.\"ActivityId\" " +
                               "join public.\"ActivitySchedules\" ss " +
                                   "on ss.\"Id\" = st.\"ScheduleId\" " +
                               "where Date(st.\"ExpirationDateEnd\") = Date('" + dateString + "') " +
                           "), " +
                           "rowCnt as ( " +
                               "select ur.*, ad.\"Address1\", " +
                                   "case " +
                                       "when ur.\"ExperienceTypeId\" = 1 then ad.\"Address1\" " +
                                       "when ur.\"ExperienceTypeId\" = 2 then 'Online' " +
                                       "else '' " +
                                   "end as \"Address\", " +
                                   "ai.\"ImageLocation\", " +
                                   "Row_Number() over ( " +
                                       "partition by ur.\"customerId\", ur.\"Id\", ur.\"scheduleId\", ur.\"DateEnd\" " +
                                       "order by ur.\"customerId\", ur.\"Id\", ur.\"scheduleId\", ur.\"DateEnd\", ai.\"Order\" " +
                                   ") as \"RowCnt\" " +
                               "from uniqueRows ur " +
                               "join public.\"ActivityAddress\" ad " +
                                   "on ad.\"ActivityId\" = ur.\"Id\" " +
                               "join public.\"ActivityImages\" ai " +
                                   "on ai.\"ActivityId\" = ur.\"Id\" " +
                           "), " +
                           "withImages as ( " +
                               "select rc.\"customerId\", rc.\"DateStart\", rc.\"DateEnd\", rc.\"Name\", " +
                                   "rc.\"Email\", rc.\"Title\", rc.\"Handler\", rc.\"Price\", rc.\"APrice\", " +
                                   "rc.\"Id\", rc.\"scheduleId\", rc.\"Address\", rc.\"ImageLocation\" " +
                               "from rowCnt rc " +
                               "where rc.\"RowCnt\" = 1 " +
                           ") " +
                           "select wi.*, " +
                               "Coalesce(Trunc((Sum(rv.\"Rating\"::decimal) / Count(rv.\"Rating\")),1),0) \"Rating\", " +
                               "Coalesce(Count(rv.\"Rating\"),0) \"Cnt\" " +
                           "from withImages wi " +
                           "left join public.\"Reviews\" rv " +
                               "on rv.\"ActivityId\" = wi.\"Id\" " +
                           "group by wi.\"customerId\", wi.\"DateStart\", wi.\"DateEnd\", wi.\"Name\", " +
                               "wi.\"Email\", wi.\"Title\", wi.\"Handler\", wi.\"Price\", " +
                               "wi.\"APrice\", wi.\"Id\", wi.\"scheduleId\", wi.\"Address\", wi.\"ImageLocation\" ";
			
			IList<ExpiredStudentDTO> listResult = new List<ExpiredStudentDTO>();

			using(var command = applicationContext.Database.GetDbConnection().CreateCommand())
			{
				command.CommandText = query;
				command.CommandType = System.Data.CommandType.Text;

				applicationContext.Database.OpenConnection();
				
				using(var dr = await command.ExecuteReaderAsync())
				{
					if(dr.HasRows)
					{
						var dt = new DataTable();
						dt.Load(dr);

						listResult = dt.AsEnumerable().Select(item => new ExpiredStudentDTO {
							DateEnd = Convert.ToDateTime(item["DateEnd"]),
							DateStart = Convert.ToDateTime(item["DateStart"]),
							Email = item["Email"].ToString() ?? string.Empty,
							FirstName = item["Name"].ToString() ?? string.Empty,
							Handler = item["Handler"].ToString() ?? string.Empty,
							Price = Convert.ToDecimal(item["Price"]),
							Title = item["Title"].ToString() ?? string.Empty,
							Address = item["Address"].ToString() ?? string.Empty,
							APrice = item["APrice"].ToString() ?? string.Empty,
							Count = Convert.ToInt32(item["Cnt"]),
							ImageLocation = item["ImageLocation"].ToString() ?? string.Empty,
							Rating = Convert.ToDecimal(item["Rating"])
						}).ToList();
					}
				}
			}

			return AppResult<IEnumerable<ExpiredStudentDTO>>.CreateSucceeded(listResult, "Successfully get expiring students");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<ExpiredStudentDTO>>.CreateFailed(ex, "An error occured when trying to get expiring students");            
		}
    }
}