using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivitySchedule;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class StudentEntity : GenericEntity<Student>, IStudent
{
	private readonly ApplicationContext applicationContext;

	public StudentEntity(ApplicationContext applicationContext)
		: base(applicationContext)
	{
		this.applicationContext = applicationContext;
	}

	public async Task<AppResult<IEnumerable<DisburseStudentDTO>>> GetAllStudentsToDisburse()
	{
		try
		{
			var now = DateTime.Now;
			var dateString = now.ToString("yyyy-MM-dd");

			string query = "with summary as " +
						   "( " +
								"select a.\"Id\" as \"TransactionId\", a.\"ActivityId\", a.\"UnitCount\", a.\"UnitPrice\", " +
									"c.\"Name\", c.\"Id\" as \"StudentId\", c.\"NumberOfSessions\", c.\"SessionsAttended\", " +
									"d.\"Date\", a.\"IsInclusivePayment\" as \"IsInclusivePayment\", " +
									"Row_Number() over (partition by a.\"Id\", c.\"Id\" order by a.\"Id\", c.\"Id\", d.\"Date\" desc) as \"RowCnt\", " +
									"a.\"PerUnitDisburseAmount\", a.\"TotalDisburseAmount\" " +
								"from public.\"PurchaseOrders\" a " +
								"join public.\"OngoingActivities\" b " +
									"on a.\"Id\" = b.\"PurchaseOrderId\" " +
								"join public.\"Students\" c " +
									"on c.\"OngoingActivityId\" = b.\"Id\" " +
								"join public.\"StudentAttendances\" d " +
									"on c.\"Id\" = d.\"StudentId\" " +
								"where c.\"IsDisbursement\" = false and c.\"SessionsAttended\" >= c.\"NumberOfSessions\" " +
									"and d.\"IsPresent\" = true and a.\"IsInclusivePayment\" = false and c.\"ExpirationDateEnd\" = '-infinity' " +
							") " +
							"select \"TransactionId\", \"IsInclusivePayment\", ac.\"CreatedBy\" as \"MakerId\", \"ActivityId\", \"StudentId\", " +
								"\"UnitCount\", \"UnitPrice\", \"Name\", \"NumberOfSessions\", \"SessionsAttended\", " +
								"\"PerUnitDisburseAmount\", \"TotalDisburseAmount\", ac.\"Title\" " +
							"from summary sm " +
							"join public.\"Activities\" ac " +
								"on ac.\"Id\" = sm.\"ActivityId\" " +
							"where \"RowCnt\" = 1 ";

			IList<DisburseStudentDTO> listResult = new List<DisburseStudentDTO>();
			
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

						listResult                = dt.AsEnumerable().Select(item => new DisburseStudentDTO {
							Name                  = item["Name"].ToString() ?? string.Empty,
							NumberOfSessions      = Convert.ToInt32(item["NumberOfSessions"]),
							SessionsAttended      = Convert.ToInt32(item["SessionsAttended"]),
							StudentId             = Convert.ToInt32(item["StudentId"]),
							TransactionId         = Convert.ToInt32(item["TransactionId"]),
							UnitCount             = Convert.ToInt32(item["UnitCount"]),
							UnitPrice             = Convert.ToDecimal(item["UnitPrice"]),
							ActivityId            = Convert.ToInt32(item["ActivityId"]),
							MakerId               = Convert.ToInt32(item["MakerId"]),
							IsInclusivePayment    = Convert.ToBoolean(item["IsInclusivePayment"]),
							PerUnitDisburseAmount = Convert.ToDecimal(item["PerUnitDisburseAmount"]),
							TotalDisburseAmount   = Convert.ToDecimal(item["TotalDisburseAmount"]),
							Title 				  = item["Title"].ToString() ?? string.Empty
						}).ToList();
					}
				}
			}

			return AppResult<IEnumerable<DisburseStudentDTO>>.CreateSucceeded(listResult, "Successfully get students need to disburse");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<DisburseStudentDTO>>.CreateFailed(ex, "An error occured when trying to get students need to disburse");
		}
	}

	public async Task<AppResult<IEnumerable<Student>>> UpdateStudentsDisbursementStatus(IEnumerable<Student> students)
	{
		try
		{
			foreach(var student in students)
			{
				var entity = await applicationContext.Students.FindAsync(student.Id);
				if(entity != null)
				{
					entity.IsDisbursement = student.IsDisbursement;
				}
			}

			await applicationContext.SaveChangesAsync();

			return AppResult<IEnumerable<Student>>.CreateSucceeded(students, "Successfully update purchase orders");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<Student>>.CreateFailed(ex, "An error occured when trying to update purchase orders");
		}
	}

	public async Task<AppResult<IEnumerable<DisburseStudentDTO>>> GetAllInclusiveStudentsToDisburse()
	{
		try
		{
			string query = "with summary as ( " +
							"select a.\"Id\" as \"TransactionId\", a.\"ActivityId\", a.\"UnitCount\", a.\"UnitPrice\", " +
								"c.\"Name\", c.\"Id\" as \"StudentId\", c.\"NumberOfSessions\", c.\"SessionsAttended\", " +
								"a.\"IsInclusivePayment\" as \"IsInclusivePayment\", a.\"PerUnitDisburseAmount\", " +
								"a.\"TotalDisburseAmount\" " +
							"from public.\"PurchaseOrders\" a " +
							"join public.\"OngoingActivities\" b " +
								"on a.\"Id\" = b.\"PurchaseOrderId\" " +
							"join public.\"Students\" c " +
								"on c.\"OngoingActivityId\" = b.\"Id\" " +
							"where c.\"IsDisbursement\" = false and a.\"IsInclusivePayment\" = true " +
							") " +
							"select \"TransactionId\",\"IsInclusivePayment\", ac.\"CreatedBy\" as \"MakerId\", \"ActivityId\", " +
								"\"StudentId\", \"UnitCount\", \"UnitPrice\", \"Name\", \"NumberOfSessions\", \"SessionsAttended\", " +
								"\"PerUnitDisburseAmount\", \"TotalDisburseAmount\", ac.\"Title\" " +
							"from summary sm " +
							"join public.\"Activities\" ac " +
								"on ac.\"Id\" = sm.\"ActivityId\"; ";

			IList<DisburseStudentDTO> listResult = new List<DisburseStudentDTO>();
			
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

						listResult                = dt.AsEnumerable().Select(item => new DisburseStudentDTO {
							Name                  = item["Name"].ToString() ?? string.Empty,
							NumberOfSessions      = Convert.ToInt32(item["NumberOfSessions"]),
							SessionsAttended      = Convert.ToInt32(item["SessionsAttended"]),
							StudentId             = Convert.ToInt32(item["StudentId"]),
							TransactionId         = Convert.ToInt32(item["TransactionId"]),
							UnitCount             = Convert.ToInt32(item["UnitCount"]),
							UnitPrice             = Convert.ToDecimal(item["UnitPrice"]),
							ActivityId            = Convert.ToInt32(item["ActivityId"]),
							MakerId               = Convert.ToInt32(item["MakerId"]),
							IsInclusivePayment    = Convert.ToBoolean(item["IsInclusivePayment"]),
							PerUnitDisburseAmount = Convert.ToDecimal(item["PerUnitDisburseAmount"]),
							TotalDisburseAmount   = Convert.ToDecimal(item["TotalDisburseAmount"]),
							Title				  = item["Title"].ToString() ?? string.Empty
						}).ToList();
					}
				}
			}

			return AppResult<IEnumerable<DisburseStudentDTO>>.CreateSucceeded(listResult, "Successfully get students need to disburse");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<DisburseStudentDTO>>.CreateFailed(ex, "An error occured when trying to get students need to disburse");
		}
	}

	public async Task<AppResult<IEnumerable<StudentDTO>>> GetCompletedStudentById(int customerId)
	{
		try
		{
			string query = "\tSELECT a.\"Id\", a.\"CustomerId\", a.\"ActivityId\", a.\"ScheduleId\", a.\"Name\", a.\"StudentNo\", a.\"NumberOfSessions\", a.\"SessionsAttended\", a.\"Remarks\", a.\"Status\", a.\"NumberOfBacktracking\", a.\"IsDisbursement\", a.\"HasReview\", a.\"ExpirationDateEnd\", a.\"ExpirationDateStart\",b.\"HasExpiration\" " +
				"FROM public.\"Students\" as a LEFT JOIN public.\"ActivitySchedules\" as b ON b.\"Id\" = a.\"ScheduleId\" " +
				"WHERE a.\"CustomerId\" =" +customerId+ "AND (a.\"HasReview\" <> true) AND " +
				"(((b.\"HasExpiration\" = 1) AND (a.\"ExpirationDateEnd\" < CURRENT_DATE) AND (a.\"ExpirationDateStart\" != '-infinity'::timestamp)) " +
				"OR ((b.\"HasExpiration\" = 2) AND (a.\"ExpirationDateEnd\" < CURRENT_DATE) AND (a.\"ExpirationDateStart\" != '-infinity'::timestamp)) " +
				"OR ((b.\"HasExpiration\" = 1) AND (a.\"SessionsAttended\" >= a.\"NumberOfSessions\")) " +
				"OR ((b.\"HasExpiration\" = 2) AND (a.\"SessionsAttended\" >= a.\"NumberOfSessions\")) " +
				"OR ((b.\"HasExpiration\" = 0) AND (a.\"SessionsAttended\" >= a.\"NumberOfSessions\")));\r\n\t\t\t\t \r\n\t\t\t\t ";

			IList<StudentDTO> listResult = new List<StudentDTO>();

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

						listResult = dt.AsEnumerable().Select(item => new StudentDTO
						{
							Id                   = Convert.ToInt32(item["Id"]),
							ActivityId           = Convert.ToInt32(item["ActivityId"]),
							CustomerId           = Convert.ToInt32(item["CustomerId"]),
							ScheduleId           = Convert.ToInt32(item["ScheduleId"]),
							Name                 = item["Name"].ToString() ?? string.Empty,
							NumberOfSessions     = Convert.ToInt32(item["NumberOfSessions"]),
							SessionsAttended     = Convert.ToInt32(item["SessionsAttended"]),
							NumberOfBackTracking = Convert.ToInt32(item["NumberOfBacktracking"]),
							ExpirationStartDate  = Convert.ToDateTime(item["ExpirationDateStart"]),
							ExpirationEndDate    = Convert.ToDateTime(item["ExpirationDateEnd"]),
							HasReview            = Convert.ToBoolean(item["HasReview"]),
							StudentNo            = item["StudentNo"].ToString() ?? "0",
							IsDisbursement       = Convert.ToBoolean(item["IsDisbursement"]),
							Remarks              = item["Remarks"].ToString() ?? string.Empty,
							Status               = item["Status"].ToString() ?? string.Empty
						}).ToList();
					}
				}
			}

			return AppResult<IEnumerable<StudentDTO>>.CreateSucceeded(listResult, "Successfully get completed students");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<StudentDTO>>.CreateFailed(ex, "An error occured when trying to get completed students");
		}
	}

	public async Task<AppResult<IEnumerable<DisburseStudentDTO>>> GetAllExpiredStudentsToDisburse()
	{
		try
		{
			var now = DateTime.Now;
			var dateString = now.ToString("yyyy-MM-dd");

			string query = "with summary as " +
						   "( " +
								"select a.\"Id\" as \"TransactionId\", a.\"ActivityId\", a.\"UnitCount\", a.\"UnitPrice\", " +
									"c.\"Name\", c.\"Id\" as \"StudentId\", c.\"NumberOfSessions\", c.\"SessionsAttended\", " +
									"a.\"IsInclusivePayment\" as \"IsInclusivePayment\", " +
									"c.\"ExpirationDateEnd\", c.\"ExpirationDateStart\", " +
									"a.\"PerUnitDisburseAmount\", a.\"TotalDisburseAmount\" " +
								"from public.\"PurchaseOrders\" a " +
								"join public.\"OngoingActivities\" b " +
									"on a.\"Id\" = b.\"PurchaseOrderId\" " +
								"join public.\"Students\" c " +
									"on c.\"OngoingActivityId\" = b.\"Id\" " +
								"where c.\"IsDisbursement\" = false " +
									"and a.\"IsInclusivePayment\" = false and c.\"ExpirationDateEnd\" != '-infinity' " +
							") " +
							"select \"TransactionId\", \"IsInclusivePayment\", ac.\"CreatedBy\" as \"MakerId\", \"ActivityId\", \"StudentId\", " +
								"\"UnitCount\", \"UnitPrice\", \"Name\", \"NumberOfSessions\", \"SessionsAttended\", " +
								"\"PerUnitDisburseAmount\", \"TotalDisburseAmount\", \"ExpirationDateEnd\", \"ExpirationDateStart\", " +
								"ac.\"Title\" " +
							"from summary sm " +
							"join public.\"Activities\" ac " +
								"on ac.\"Id\" = sm.\"ActivityId\" " +
							"where Date(\"ExpirationDateEnd\") < Date('" + dateString +"') ";
			
			IList<DisburseStudentDTO> listResult = new List<DisburseStudentDTO>();
			
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

						listResult                = dt.AsEnumerable().Select(item => new DisburseStudentDTO {
							Name                  = item["Name"].ToString() ?? string.Empty,
							StudentId             = Convert.ToInt32(item["StudentId"]),
							TransactionId         = Convert.ToInt32(item["TransactionId"]),
							UnitCount             = Convert.ToInt32(item["UnitCount"]),
							UnitPrice             = Convert.ToDecimal(item["UnitPrice"]),
							ActivityId            = Convert.ToInt32(item["ActivityId"]),
							MakerId               = Convert.ToInt32(item["MakerId"]),
							IsInclusivePayment    = Convert.ToBoolean(item["IsInclusivePayment"]),
							PerUnitDisburseAmount = Convert.ToDecimal(item["PerUnitDisburseAmount"]),
							TotalDisburseAmount   = Convert.ToDecimal(item["TotalDisburseAmount"]),
							Title = item["Title"].ToString() ?? string.Empty
						}).ToList();
					}
				}
			}

			return AppResult<IEnumerable<DisburseStudentDTO>>.CreateSucceeded(listResult, "Successfully get students need to disburse");

		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<DisburseStudentDTO>>.CreateFailed(ex, "An error occured when trying to get students need to disburse");
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
											"ct.\"FirstName\", ct.\"LastName\", ct.\"Email\", " +
											"ac.\"Title\", ac.\"Handler\", ss.\"Price\", ac.\"Price\" \"APrice\", " +
											"ac.\"Id\", ac.\"ExperienceTypeId\", ss.\"Id\" \"scheduleId\" " +
								"from public.\"Students\" st " +
								"join public.\"Customers\" ct " +
									"on ct.\"Id\" = st.\"CustomerId\" " +
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
								"select rc.\"customerId\", rc.\"DateStart\", rc.\"DateEnd\", rc.\"FirstName\", rc.\"LastName\", " +
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
							"group by wi.\"customerId\", wi.\"DateStart\", wi.\"DateEnd\", wi.\"FirstName\", wi.\"LastName\", " +
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
							FirstName = item["FirstName"].ToString() ?? string.Empty,
							Handler = item["Handler"].ToString() ?? string.Empty,
							LastName = item["LastName"].ToString() ?? string.Empty,
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

	public async Task<AppResult<IEnumerable<StudentDTO>>> GetAllStudentById(int customerId)
	{
		try
		{
			string query = "SELECT a.\"Id\", a.\"CustomerId\", a.\"ActivityId\", a.\"ScheduleId\", a.\"Name\", a.\"NumberOfSessions\", a.\"SessionsAttended\", a.\"NumberOfBacktracking\", " +
				"a.\"ExpirationDateEnd\", a.\"ExpirationDateStart\",a.\"CreatedOn\" as PurchaseDate, a.\"HasReview\", b.\"Id\" as StudentAttendanceId, b.\"StudentId\", b.\"IsPresent\", b.\"Date\", c.\"Id\" as ActivityScheduleId, c.\"IsSetSession\", " +
				"c.\"HasExpiration\", d.\"Title\" \r\nFROM public.\"Students\" as a " +
				"JOIN public.\"ActivitySchedules\" as c ON c.\"Id\" = a.\"ScheduleId\"\r\n" +
				"JOIN public.\"Activities\" as d ON d.\"Id\" = a.\"ActivityId\"\r\n" +
				"LEFT JOIN ( SELECT sa.\"Id\", sa.\"StudentId\", sa.\"IsPresent\",sa.\"Date\"\r\n" +
				"FROM public.\"StudentAttendances\" as sa WHERE sa.\"IsPresent\" = true\r\n" +
				"AND sa.\"Date\" = (SELECT MAX(sa_sub.\"Date\") FROM public.\"StudentAttendances\" as sa_sub " +
				"WHERE sa_sub.\"StudentId\" = sa.\"StudentId\"\r\nAND sa_sub.\"IsPresent\" = true)) as b ON b.\"StudentId\" = a.\"Id\"\r\n" +
				"WHERE a.\"CustomerId\" = " + customerId + "ORDER BY a.\"ExpirationDateEnd\", b.\"Date\";";

			IList<StudentDTO> listResult = new List<StudentDTO>();


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
						//Get Student
						listResult = dt.AsEnumerable().Select(item => new StudentDTO
						{
							Id                   = Convert.ToInt32(item["Id"]),
							ActivityId           = Convert.ToInt32(item["ActivityId"]),
							CustomerId           = Convert.ToInt32(item["CustomerId"]),
							ScheduleId           = Convert.ToInt32(item["ScheduleId"]),
							Name                 = item["Name"].ToString() ?? string.Empty,
							NumberOfSessions     = Convert.ToInt32(item["NumberOfSessions"]),
							SessionsAttended     = Convert.ToInt32(item["SessionsAttended"]),
							NumberOfBackTracking = Convert.ToInt32(item["NumberOfBacktracking"]),
							ExpirationStartDate  = Convert.ToDateTime(item["ExpirationDateStart"]),
							ExpirationEndDate    = Convert.ToDateTime(item["ExpirationDateEnd"]),
							HasReview            = Convert.ToBoolean(item["HasReview"]),
							PurchaseDate         = Convert.ToDateTime(item["PurchaseDate"]),
							Title                = item["Title"].ToString() ?? string.Empty,
							activitySchedule     = new ActivityScheduleDTO
							{
								Id            = Convert.ToInt32(item["ActivityScheduleId"]),
								HasExpiration = Convert.ToInt32(item["HasExpiration"]),
								IsSetSession  = Convert.ToBoolean(item["IsSetSession"])
							},
							studentAttendance = new StudentAttendanceDTO
							{
								Id        = item["StudentAttendanceId"] != DBNull.Value ? Convert.ToInt32(item["StudentAttendanceId"]) : 0,
								Date      = item["Date"] != DBNull.Value ? Convert.ToDateTime(item["Date"]) : DateTime.MinValue,
								IsPresent = item["IsPresent"] != DBNull.Value ? Convert.ToBoolean(item["IsPresent"]) : false,
								StudentId = item["StudentId"] != DBNull.Value ? Convert.ToInt32(item["StudentId"]) : 0,
							}
						}).ToList();
					}
				}
			}

			return AppResult<IEnumerable<StudentDTO>>.CreateSucceeded(listResult, "Successfully get completed students");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<StudentDTO>>.CreateFailed(ex, "An error occured when trying to get completed students");
		}
	}

	public async Task<AppResult<IEnumerable<StudentDTO>>> GetEnrolleeMasterList(int providerId, int? count, int? skip)
	{
		try
		{
			int limitCount = count.HasValue ? count.Value : int.MaxValue;
			int skipCount = skip.HasValue ? skip.Value : 0;

			string query = "WITH StudentTotals AS ( SELECT a.\"FamilyMemberId\", SUM(a.\"NumberOfSessions\") AS \"TotalNumberOfSessions\", SUM(a.\"SessionsAttended\") AS \"TotalSessionsAttended\", MAX(a.\"CreatedOn\") AS \"LatestCreatedOn\"\r\n" +
				"FROM public.\"Students\" AS a\r\n" +
				"JOIN public.\"Activities\" AS b ON b.\"Id\" = a.\"ActivityId\"\r\n" +
				"WHERE b.\"CreatedBy\" = "+providerId+" GROUP BY a.\"FamilyMemberId\"\r\n" +
				")\r\n" +
				"SELECT\r\n" +
				"a.\"Id\",a.\"CustomerId\", a.\"ActivityId\", a.\"ScheduleId\", a.\"Name\", a.\"StudentNo\", st.\"TotalNumberOfSessions\" AS \"FamilyTotalNumberOfSessions\",\r\n" +
				"st.\"TotalSessionsAttended\" AS \"FamilyTotalSessionsAttended\", a.\"Remarks\", a.\"Status\", a.\"CreatedOn\", a.\"FamilyMemberId\",\r\n" +
				"b.\"Title\", b.\"CreatedBy\", c.\"Name\" AS \"ChildName\", c.\"Gender\",\r\n" +
				"EXTRACT(YEAR FROM AGE(current_date, DATE(c.\"BirthYear\" || '-' || c.\"BirthMonth\" || '-01'))) AS \"ChildAge\", d.\"Email\"\r\n" +
				"FROM public.\"Students\" AS a\r\nJOIN public.\"Activities\" AS b ON b.\"Id\" = a.\"ActivityId\"\r\n" +
				"JOIN public.\"FamilyMembers\" AS c ON c.\"Id\" = a.\"FamilyMemberId\"\r\n" +
				"JOIN public.\"Customers\" AS d ON d.\"Id\" = a.\"CustomerId\"\r\n" +
				"JOIN StudentTotals AS st ON st.\"FamilyMemberId\" = a.\"FamilyMemberId\" AND st.\"LatestCreatedOn\" = a.\"CreatedOn\"\r\n" +
				"WHERE b.\"CreatedBy\" = "+providerId+"\r\n" +
				"ORDER BY a.\"CreatedOn\";";

			IList<StudentDTO> listResult = new List<StudentDTO>();


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
						//Get Enrollee Master List
						listResult = dt.AsEnumerable().Select(item => new StudentDTO
						{
							Id                = Convert.ToInt32(item["Id"]),
							ActivityId        = Convert.ToInt32(item["ActivityId"]),
							CustomerId        = Convert.ToInt32(item["CustomerId"]),
							ScheduleId        = Convert.ToInt32(item["ScheduleId"]),
							Name              = item["Name"].ToString() ?? string.Empty,
							Age               = Convert.ToInt32(item["ChildAge"]),
							Gender            = item["Gender"].ToString() ?? string.Empty,
							NumberOfSessions  = Convert.ToInt32(item["FamilyTotalNumberOfSessions"]),
							SessionsAttended  = Convert.ToInt32(item["FamilyTotalSessionsAttended"]),
							ActivityTitle     = item["Title"].ToString() ?? string.Empty,
							Email             = item["Email"].ToString() ?? string.Empty,
							StudentNo         = item["StudentNo"].ToString() ?? string.Empty,
							Remarks           = item["Remarks"].ToString() ?? string.Empty,
							FamilyMemberId    = Convert.ToInt32(item["FamilyMemberId"]),
						}).Skip(skipCount).Take(limitCount).ToList();
					}
				}
			}

			return AppResult<IEnumerable<StudentDTO>>.CreateSucceeded(listResult, "Successfully get completed students");
		}
		catch (Exception ex)
		{
			return AppResult<IEnumerable<StudentDTO>>.CreateFailed(ex, "An error occured when trying to get completed students");
		}
	}

	public async Task<AppResult<IEnumerable<StudentDTO>>> GetEnrolledStudents(int? providerId, string searchValue, int searchBy, int? count, int? skip)
	{
        try
        {
            int limitCount = count.HasValue ? count.Value : int.MaxValue;
            int skipCount = skip.HasValue ? skip.Value : 0;
            string whereClause = string.Empty;
			if (providerId.HasValue)
			{
                whereClause += "WHERE a.\"CreatedBy\" = @providerId";
            }
            switch (searchBy)
			{
				case 0:
                    whereClause += " ORDER BY b.\"SessionsAttended\" >= b.\"NumberOfSessions\", b.\"ExpirationDateStart\"";
                    break;
                case 1:
					whereClause += " AND b.\"Name\" ILIKE '%' ||"+"'"+searchValue +"'"+"|| '%' ORDER BY b.\"SessionsAttended\" >= b.\"NumberOfSessions\", b.\"ExpirationDateStart\"";
					break;
				case 2: whereClause += " AND (b.\"SessionsAttended\" < b.\"NumberOfSessions\" AND (b.\"ExpirationDateEnd\" >= CURRENT_DATE OR b.\"ExpirationDateEnd\" = '-infinity'))";
					break;
				case 3: whereClause += " AND ((b.\"SessionsAttended\" >= b.\"NumberOfSessions\" AND c.\"HasExpiration\" = 0) \r\n" +
						"OR((c.\"HasExpiration\" = 1 AND b.\"ExpirationDateEnd\" < CURRENT_DATE AND b.\"ExpirationDateStart\" != '-infinity')OR \r\n" +
						"(c.\"HasExpiration\" = 2 AND b.\"ExpirationDateEnd\" < CURRENT_DATE AND b.\"ExpirationDateStart\" != '-infinity')OR \r\n" +
						"(c.\"HasExpiration\" = 2 AND b.\"SessionsAttended\" >= b.\"NumberOfSessions\" AND b.\"ExpirationDateEnd\" != '-infinity')));";
					break;
				default:
					break;
			}

			string query = "SELECT a.\"Id\", a.\"Title\",  a.\"CreatedBy\", b.\"Id\" as \"StudentId\",\r\n" +
				"b.\"Name\", b.\"NumberOfSessions\", b.\"SessionsAttended\", b.\"StudentNo\",\r\n" +
				"b.\"Remarks\", b.\"ExpirationDateEnd\", b.\"ExpirationDateStart\", c.\"Id\" as \"ScheduleId\", c.\"HasExpiration\"\r\n" +
				"FROM public.\"Activities\" as a \r\n" +
				"JOIN public.\"Students\" as b ON b.\"ActivityId\" = a.\"Id\"\r\n" +
				"JOIN public.\"ActivitySchedules\" as c ON \"c\".\"Id\" = b.\"ScheduleId\"\r\n" +
                "" + whereClause + ";";
            IList <StudentDTO> listResult = new List<StudentDTO>();

            using (var command = applicationContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = System.Data.CommandType.Text;
                if (providerId.HasValue)
                {
                    var parameterCustomerId = new NpgsqlParameter("providerId", providerId.Value);
                    command.Parameters.Add(parameterCustomerId);
                }
                applicationContext.Database.OpenConnection();

                using (var dr = await command.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        var dt = new DataTable();
                        dt.Load(dr);
                        //Get Enrolled Student List
                        listResult = dt.AsEnumerable().Select(item => new StudentDTO
                        {
                            Id                  = Convert.ToInt32(item["StudentId"]),
                            ActivityId          = Convert.ToInt32(item["Id"]),
                            ScheduleId          = Convert.ToInt32(item["ScheduleId"]),
                            Name                = item["Name"].ToString() ?? string.Empty,
                            NumberOfSessions    = Convert.ToInt32(item["NumberOfSessions"]),
                            SessionsAttended    = Convert.ToInt32(item["SessionsAttended"]),
                            ActivityTitle       = item["Title"].ToString() ?? string.Empty,
                            StudentNo           = item["StudentNo"].ToString() ?? string.Empty,
                            Remarks             = item["Remarks"].ToString() ?? string.Empty,
							ExpirationEndDate   = Convert.ToDateTime(item["ExpirationDateEnd"]),
							ExpirationStartDate = Convert.ToDateTime(item["ExpirationDateStart"]),
							HasExpiration       = Convert.ToInt32(item["HasExpiration"]),

                        }).Skip(skipCount).Take(limitCount).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<StudentDTO>>.CreateSucceeded(listResult, "Successfully get completed students");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentDTO>>.CreateFailed(ex, "An error occured when trying to get completed students");
        }
    }
}