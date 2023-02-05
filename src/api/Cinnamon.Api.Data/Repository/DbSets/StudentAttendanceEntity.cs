using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

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
}