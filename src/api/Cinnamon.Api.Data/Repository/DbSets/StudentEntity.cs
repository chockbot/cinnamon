using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivitySchedule;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;
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
			string query = "with summary as ( " +
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
							"where c.\"IsDisbursement\" = false and a.\"Status\" = 1 and c.\"SessionsAttended\" >= c.\"NumberOfSessions\" " +
								"and d.\"IsPresent\" = true and a.\"IsInclusivePayment\" = false " +
							") " +
							"select \"TransactionId\", \"IsInclusivePayment\", ac.\"CreatedBy\" as \"MakerId\", \"ActivityId\", \"StudentId\", " +
							"\"UnitCount\", \"UnitPrice\", \"Name\", \"NumberOfSessions\", \"SessionsAttended\", " +
							"Date(\"Date\" + Interval '2 days') as \"EndDate\", Date(Current_Timestamp) as \"DateNow\", " +
							"\"PerUnitDisburseAmount\", \"TotalDisburseAmount\" " +
							"from summary sm " +
							"join public.\"Activities\" ac " +
								"on ac.\"Id\" = sm.\"ActivityId\" " +
							"where \"RowCnt\" = 1 and Date(\"Date\" + Interval '2 days') <= Date(Current_Timestamp) ";

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
							TotalDisburseAmount   = Convert.ToDecimal(item["TotalDisburseAmount"])
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
							"where c.\"IsDisbursement\" = false and a.\"Status\" = 1 and a.\"IsInclusivePayment\" = true " +
							") " +
							"select \"TransactionId\",\"IsInclusivePayment\", ac.\"CreatedBy\" as \"MakerId\", \"ActivityId\", " +
								"\"StudentId\", \"UnitCount\", \"UnitPrice\", \"Name\", \"NumberOfSessions\", \"SessionsAttended\", " +
								"\"PerUnitDisburseAmount\", \"TotalDisburseAmount\" " +
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
							TotalDisburseAmount   = Convert.ToDecimal(item["TotalDisburseAmount"])
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
				"(((b.\"HasExpiration\" = 1) AND (a.\"ExpirationDateEnd\" <= CURRENT_DATE) AND (a.\"ExpirationDateStart\" != '-infinity'::timestamp)) " +
				"OR ((b.\"HasExpiration\" = 2) AND (a.\"ExpirationDateEnd\" <= CURRENT_DATE) AND (a.\"ExpirationDateStart\" != '-infinity'::timestamp)) " +
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

	public async Task<AppResult<IEnumerable<StudentDTO>>> GetAllStudentById(int customerId)
	{
		try
		{
			string query = "SELECT a.\"Id\", a.\"CustomerId\", a.\"ActivityId\", a.\"ScheduleId\", a.\"Name\", a.\"NumberOfSessions\", a.\"SessionsAttended\", a.\"NumberOfBacktracking\", " +
				"a.\"ExpirationDateEnd\", a.\"ExpirationDateStart\", a.\"HasReview\", b.\"Id\" as StudentAttendanceId, b.\"StudentId\", b.\"IsPresent\", b.\"Date\", c.\"Id\" as ActivityScheduleId, c.\"IsSetSession\", " +
				"c.\"HasExpiration\"\r\nFROM public.\"Students\" as a " +
				"JOIN public.\"ActivitySchedules\" as c ON c.\"Id\" = a.\"ScheduleId\"\r\n" +
				"LEFT JOIN ( SELECT sa.\"Id\", sa.\"StudentId\", sa.\"IsPresent\",sa.\"Date\"\r\n" +
				"FROM public.\"StudentAttendances\" as sa WHERE sa.\"IsPresent\" = true\r\n" +
				"AND sa.\"Date\" = (SELECT MAX(sa_sub.\"Date\") FROM public.\"StudentAttendances\" as sa_sub " +
				"WHERE sa_sub.\"StudentId\" = sa.\"StudentId\"\r\nAND sa_sub.\"IsPresent\" = true)) as b ON b.\"StudentId\" = a.\"Id\"\r\n" +
				"WHERE a.\"CustomerId\" = " + customerId+";";

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
							HasReview			 = Convert.ToBoolean(item["HasReview"]),
							activitySchedule     = new ActivityScheduleDTO
							{
								Id            = Convert.ToInt32(item["ActivityScheduleId"]),
								HasExpiration = Convert.ToInt32(item["HasExpiration"]),
								IsSetSession  = Convert.ToBoolean(item["IsSetSession"])
							},
							studentAttendance = new StudentAttendanceDTO
							{
								Id = item["StudentAttendanceId"] != DBNull.Value ? Convert.ToInt32(item["StudentAttendanceId"]) : 0,
								Date = item["Date"] != DBNull.Value ? Convert.ToDateTime(item["Date"]) : DateTime.MinValue,
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
}