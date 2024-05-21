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
            var result = await applicationContext.DirectStudentSessions
                                    .Where(s => s.ActivityId == activityId && s.NumberOfSessions > s.SessionsAttended)
                                    .CountAsync();
            
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
            var result = await applicationContext.DirectStudentSessions
                                    .Where(s => s.ActivityId == activityId && s.NumberOfSessions <= s.SessionsAttended)
                                    .CountAsync();
            
            return AppResult<int>.CreateSucceeded(result, "Successfully get completed student count.");
        }
        catch (Exception ex)
        {
            return AppResult<int>.CreateFailed(ex, "An error occured when getting completed student count.");
        }
    }
}