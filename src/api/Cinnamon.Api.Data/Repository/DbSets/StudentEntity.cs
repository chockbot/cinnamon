using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
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

                        listResult = dt.AsEnumerable().Select(item => new DisburseStudentDTO {
                            Name = item["Name"].ToString() ?? string.Empty,
                            NumberOfSessions = Convert.ToInt32(item["NumberOfSessions"]),
                            SessionsAttended = Convert.ToInt32(item["SessionsAttended"]),
                            StudentId = Convert.ToInt32(item["StudentId"]),
                            TransactionId = Convert.ToInt32(item["TransactionId"]),
                            UnitCount = Convert.ToInt32(item["UnitCount"]),
                            UnitPrice = Convert.ToDecimal(item["UnitPrice"]),
                            ActivityId = Convert.ToInt32(item["ActivityId"]),
                            MakerId = Convert.ToInt32(item["MakerId"]),
                            IsInclusivePayment = Convert.ToBoolean(item["IsInclusivePayment"]),
                            PerUnitDisburseAmount = Convert.ToDecimal(item["PerUnitDisburseAmount"]),
                            TotalDisburseAmount = Convert.ToDecimal(item["TotalDisburseAmount"])
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

                        listResult = dt.AsEnumerable().Select(item => new DisburseStudentDTO {
                            Name = item["Name"].ToString() ?? string.Empty,
                            NumberOfSessions = Convert.ToInt32(item["NumberOfSessions"]),
                            SessionsAttended = Convert.ToInt32(item["SessionsAttended"]),
                            StudentId = Convert.ToInt32(item["StudentId"]),
                            TransactionId = Convert.ToInt32(item["TransactionId"]),
                            UnitCount = Convert.ToInt32(item["UnitCount"]),
                            UnitPrice = Convert.ToDecimal(item["UnitPrice"]),
                            ActivityId = Convert.ToInt32(item["ActivityId"]),
                            MakerId = Convert.ToInt32(item["MakerId"]),
                            IsInclusivePayment = Convert.ToBoolean(item["IsInclusivePayment"]),
                            PerUnitDisburseAmount = Convert.ToDecimal(item["PerUnitDisburseAmount"]),
                            TotalDisburseAmount = Convert.ToDecimal(item["TotalDisburseAmount"])
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

            string query = "select distinct st.\"ExpirationDateStart\" \"DateStart\", st.\"ExpirationDateEnd\" \"DateEnd\",  " +
                                "ct.\"FirstName\", ct.\"LastName\", ct.\"Email\", " +
                                "ac.\"Title\", ac.\"Handler\", ss.\"Price\" " +
                            "from public.\"Students\" st " +
                            "join public.\"Customers\" ct " +
                                "on ct.\"Id\" = st.\"CustomerId\" " +
                            "join public.\"Activities\" ac " +
                                "on ac.\"Id\" = st.\"ActivityId\" " +
                            "join public.\"ActivitySchedules\" ss " +
                                "on ss.\"Id\" = st.\"ScheduleId\" " +
                            "where Date(st.\"ExpirationDateEnd\") = Date('" + dateString + "') ";
            
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
                            Title = item["Title"].ToString() ?? string.Empty
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