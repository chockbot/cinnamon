using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class DirectStudentAttendanceEntity : GenericEntity<DirectStudentAttendance>, IDirectStudentAttendance
{
    private readonly ApplicationContext applicationContext;

    public DirectStudentAttendanceEntity(ApplicationContext applicationContext)
        :base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<DirectStudentAttendance>>> UpdateStudentAttendance(IEnumerable<DirectStudentAttendance> studentAttendances, DateTime date)
    {
        try
        {
            var studentIdsDic = studentAttendances.ToDictionary(s => s.DirectStudentSessionId);
            var ids = studentAttendances.Select(s => s.DirectStudentSessionId);

            var studentAttendanceRes = await applicationContext.DirectStudentAttendances
                                            .Where(s => ids.Contains(s.DirectStudentSessionId) && s.Date == date)
                                            .Include(s => s.DirectStudentSession)
                                            .ToListAsync();

            foreach(var attendance in studentAttendanceRes)
            {
                if(!studentIdsDic.ContainsKey(attendance.DirectStudentSessionId)) continue;

                var attendanceToUpdate = studentIdsDic[attendance.DirectStudentSessionId];
                if(attendance.IsPresent != attendanceToUpdate.IsPresent)
                {
                    attendance.IsPresent = attendanceToUpdate.IsPresent;
                    if(attendance.IsPresent)
                    {
                        attendance.DirectStudentSession.SessionsAttended++;
                    }
                    else 
                    {
                        attendance.DirectStudentSession.SessionsAttended--;
                    }
                }
            }

            await applicationContext.SaveChangesAsync();

            return AppResult<IEnumerable<DirectStudentAttendance>>.CreateSucceeded(studentAttendanceRes, "Successfully update direct student attendance.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DirectStudentAttendance>>.CreateFailed(ex, "An error occured when updating direct student attendances.");
        }        
    }
}