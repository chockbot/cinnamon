using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class StudentAttendanceEntity : GenericEntity<StudentAttendance>, IStudentAttendance
{
    private readonly ApplicationContext applicationContext;

    public StudentAttendanceEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<StudentAttendance>>> UpdateStudentAttendance(DateTime date, IEnumerable<StudentAttendance> attendances)
    {
        try
        {
            // convert it to dictionary for faster searchching based on key id
            var attendanceToDictionary = attendances.ToDictionary(a => a.StudentId);
            var ids = attendances.Select(a => a.StudentId);

            var studentAttendance = await applicationContext.StudentAttendances
                                            .Where(a => ids.Contains(a.StudentId) && a.Date == date)
                                            .Include(s => s.Student)
                                            .ToListAsync();

            // check if student attendance have student details to update
            foreach(var attendnace in studentAttendance)
            {
                if(attendanceToDictionary.ContainsKey(attendnace.StudentId) && attendnace.Student != null)
                {
                    var student = attendnace.Student;
                    var attendanceToUpdate = attendanceToDictionary[attendnace.StudentId];

                    if(attendnace.IsPresent != attendanceToUpdate.IsPresent)
                    {
                        attendnace.IsPresent = attendanceToUpdate.IsPresent;
                        if(attendnace.IsPresent)
                        {
                            student.SessionsAttended++;
                        }
                        else {
                            student.SessionsAttended--;
                        }
                    }
                }
            }

            await applicationContext.SaveChangesAsync();

            return AppResult<IEnumerable<StudentAttendance>>.CreateSucceeded(studentAttendance, "Successfully updated student attendance");
            
        }
        catch (Exception ex)
        {   
            return AppResult<IEnumerable<StudentAttendance>>.CreateFailed(ex, "An error occured when updating student attendance");
        }
    }

    public async Task<AppResult<StudentAttendance>> GetLastStudentAttendance(int id, int activityId, int scheduleId)
    {
        try
        {
            
            var studentAttendance = await applicationContext.StudentAttendances
                                            .Where(a => a.StudentId == id && a.Student.ScheduleId == scheduleId 
                                             && a.Student.ActivityId == activityId && a.IsPresent == true)
                                            .Include(s => s.Student)
                                            .OrderBy(a => a.Date)
                                            .LastOrDefaultAsync();

            await applicationContext.SaveChangesAsync();

            return AppResult<StudentAttendance>.CreateSucceeded(studentAttendance, "Successfully updated student attendance");

        }
        catch (Exception ex)
        {
            return AppResult<StudentAttendance>.CreateFailed(ex, "An error occured when updating student attendance");
        }
    }

    public async Task<AppResult<IEnumerable<StudentAttendance>>> GetAttendanceByFamilyId(int familyId)
    {
        try
        {
            string query = "SELECT a.\"Id\", a.\"CustomerId\", a.\"ActivityId\", a.\"ScheduleId\", a.\"Name\", a.\"StudentNo\", a.\"NumberOfSessions\", \r\n" +
                "a.\"SessionsAttended\", a.\"CreatedOn\",a.\"FamilyMemberId\",b.\"IsPresent\",b.\"Date\",c.\"FirstName\",c.\"LastName\"\r\n" +
                "FROM public.\"Students\" as a JOIN public.\"StudentAttendances\" as b ON b.\"StudentId\" = a.\"Id\" \r\n" +
                "JOIN public.\"Customers\" as c ON c.\"Id\" = a.\"CustomerId\" WHERE a.\"FamilyMemberId\" = "+ familyId + " AND b.\"IsPresent\" = true; ";

            IList<StudentAttendance> listResult = new List<StudentAttendance>();


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
                        listResult = dt.AsEnumerable().Select(item => new StudentAttendance
                        {
                            Id = Convert.ToInt32(item["Id"]),
                            IsPresent = Convert.ToBoolean(item["IsPresent"]),
                            Date = Convert.ToDateTime(item["Date"]),
                            Student = new Student
                            {
                                ActivityId = Convert.ToInt32(item["ActivityId"]),
                                CustomerId = Convert.ToInt32(item["CustomerId"]),
                                ScheduleId = Convert.ToInt32(item["ScheduleId"]),
                                Name = item["Name"].ToString() ?? string.Empty,
                                NumberOfSessions = Convert.ToInt32(item["NumberOfSessions"]),
                                SessionsAttended = Convert.ToInt32(item["SessionsAttended"]),
                                FamilyMemberId = Convert.ToInt32(item["FamilyMemberId"]),
                            }
                        }).ToList();
                    }
                }
            }

            return AppResult<IEnumerable<StudentAttendance>>.CreateSucceeded(listResult, "Successfully get completed students");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<StudentAttendance>>.CreateFailed(ex, "An error occured when trying to get completed students");
        }
    }
}