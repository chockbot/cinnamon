using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class UpdateStudentAttendanceHandler : IUpdateStudentAttendanceHandler
{
    private readonly IGetOwnedActivitiesHandler getOwnedActivitiesHandler;
    private readonly IStudentAttendanceData studentAttendanceData;
    private readonly IDirectStudentData directStudentData;

    public UpdateStudentAttendanceHandler(IGetOwnedActivitiesHandler getOwnedActivitiesHandler, 
        IStudentAttendanceData studentAttendanceData, IDirectStudentData directStudentData)
    {
        this.getOwnedActivitiesHandler = getOwnedActivitiesHandler;
        this.studentAttendanceData = studentAttendanceData;
        this.directStudentData = directStudentData;
    }

    public AppResult<UpdateStudentAttendanceResult> Execute(UpdateStudentAttendanceArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateStudentAttendanceResult>.CreateFailed(ex, "An error occured in UpdateStudentAttendanceHandler");
        }
    }

    public async Task<AppResult<UpdateStudentAttendanceResult>> ExecuteAsync(UpdateStudentAttendanceArgs args)
    {
        try
        {
            // get only activity ids and schedules ids need to filter
            var activityScheduleIds = await GetOnlyRequiredIds(args.Students);
            if(!activityScheduleIds.Succeeded || activityScheduleIds.Result == null)
            {
                return AppResult<UpdateStudentAttendanceResult>.CreateFailed(activityScheduleIds.Error.Exception, activityScheduleIds.Message);
            }

            // get associated students based in activity and schedule
            var studentsData = await studentAttendanceData.GetAllStudentAttendance(new Framework.ApiCommand.ApiData.StudentAttendance.Request.GetAllStudentAttendanceArgs {
                ActivityIds = activityScheduleIds.Result.Select(i => i.Value),
                ScheduleIds  = activityScheduleIds.Result.Select(i => i.Key),
                Date = args.Date.ToString("yyyyMMdd"),
            });
            if(!studentsData.Succeeded || studentsData.Result == null || !studentsData.Result.IsSuccess)
            {
                return AppResult<UpdateStudentAttendanceResult>.CreateFailed(
                    new ApplicationException(studentsData.Result?.ErrorInfo?.Message), studentsData.Message);
            }

            // update only enrolled students associated in the activity and schedule
            // convert to dictionary for faster search
            var studentToDictionary = studentsData.Result.Result.ToDictionary(s => s.StudentId);
            var filteredStudents = args.Students.Where(s => studentToDictionary.ContainsKey(s.StudentId) && 
                                    s.StudentType == Framework.Enums.StudentType.Cinnamon);

            var updateStudentRes = await studentAttendanceData.UpdateAttendance(new Framework.ApiCommand.ApiData.StudentAttendance.Request.UpdateAttendanceArgs {
                Date = args.Date,
                StudentAttendaces = filteredStudents.Select(s => {
                    return new Framework.ApiCommand.ApiData.StudentAttendance.Request.UpdateAttendanceArgs.UpdateAttendance {
                        IsPresent = s.IsPresent,
                        StudentId = s.StudentId
                    };
                })
            });
            if(!updateStudentRes.Succeeded || updateStudentRes.Result == null || !updateStudentRes.Result.IsSuccess)
            {
                return AppResult<UpdateStudentAttendanceResult>.CreateFailed(
                    new ApplicationException(updateStudentRes.Result?.ErrorInfo?.Message), updateStudentRes.Message);
            }

            // for manual students need to update
            var directStudentsAttendance = await directStudentData.StudentAttendance(new Framework.ApiCommand.ApiData.DirectStudent.Request.StudentAttendanceArgs {
                ActivityIds = activityScheduleIds.Result.Select(i => i.Value),
                ScheduleIds = activityScheduleIds.Result.Select(i => i.Key),
                Date = args.Date.ToString("yyyyMMdd"),
            });
            if(!directStudentsAttendance.Succeeded || directStudentsAttendance.Result == null || !directStudentsAttendance.Result.IsSuccess)
            {
                return AppResult<UpdateStudentAttendanceResult>.CreateFailed(
                    new ApplicationException(directStudentsAttendance.Result?.ErrorInfo?.Message), directStudentsAttendance.Message);
            }

            // update only enrolled students associated in the activity and schedule
            // convert to dictionary for faster search
            var directStudentToDictionary = directStudentsAttendance.Result.Result.ToDictionary(s => s.StudentId);
            var directFilteredStudents = args.Students.Where(s => directStudentToDictionary.ContainsKey(s.StudentId) && 
                                            s.StudentType == Framework.Enums.StudentType.Manual);

            var updateDirectStudentRes = await directStudentData.UpdateStudentAttendance(new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateStudentAttendanceBulkArgs {
                Date = args.Date.Date,
                StudentAttendances = directFilteredStudents.Select(s => new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateStudentAttendanceBulkArgs.UpdateStudentAttendance {
                    DirectStudentSessionId = s.StudentId,
                    IsPresent = s.IsPresent
                })
            });
            if(!updateDirectStudentRes.Succeeded || updateDirectStudentRes.Result is null || !updateDirectStudentRes.Result.IsSuccess)
            {
                return AppResult<UpdateStudentAttendanceResult>.CreateFailed(
                    new ApplicationException(updateDirectStudentRes.Result?.ErrorInfo?.Message), updateDirectStudentRes.Message);
            }

            var updatedStudents = updateStudentRes.Result.Result.Select(s => {
                return new UpdateStudentAttendanceResult.UpdatedStudentDetails {
                    ActivityId = s.Student.ActivityId,
                    IsPresent = s.IsPresent,
                    ScheduleId = s.Student.ScheduleId,
                    StudentId = s.StudentId
                };
            }).ToList();

            return AppResult<UpdateStudentAttendanceResult>.CreateSucceeded(new UpdateStudentAttendanceResult {
                StudentAttendaces = updateStudentRes.Result.Result.Select(s => {
                    return new UpdateStudentAttendanceResult.UpdatedStudentDetails {
                        ActivityId = s.Student.ActivityId,
                        IsPresent = s.IsPresent,
                        ScheduleId = s.Student.ScheduleId,
                        StudentId = s.StudentId
                    };
                })
            }, "Successfully update student attendance");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateStudentAttendanceResult>.CreateFailed(ex, "An error occured in UpdateStudentAttendanceHandler");
        }
    }

    private async Task<AppResult<IDictionary<int,int>>> GetOnlyRequiredIds(IEnumerable<UpdateStudentAttendanceArgs.StudentDetails> students)
    {
        try
        {
            var ownedAtivitiesResult = await getOwnedActivitiesHandler.ExecuteAsync(new ActivityService.Interactors.GetOwnedActivitiesArgs {
                IncludeAtivitySchedules = true
            });
            if(!ownedAtivitiesResult.Succeeded || ownedAtivitiesResult.Result == null)
            {
                return AppResult<IDictionary<int,int>>.CreateFailed(ownedAtivitiesResult.Error.Exception, ownedAtivitiesResult.Message);
            }

            // convert activity schedule Ids to dictionary
            // to get the data faster, key = scheduleId, value = activityId
            IDictionary<int,int> activitySchedules = new Dictionary<int,int>();
            foreach(var activity in ownedAtivitiesResult.Result.Activities)
            {
                foreach(var schedule in activity.ActivitySchedules)
                {
                    activitySchedules.Add(schedule.Id, activity.Id);
                }
            }

            IDictionary<int,int> result = new Dictionary<int,int>();
            foreach(var student in students)
            {
                // check if activity is existed and schedule is associated to activity
                if(activitySchedules.ContainsKey(student.ScheduleId) && activitySchedules[student.ScheduleId] == student.ActivityId)
                {
                    // check if the ids already in the result
                    if(!result.ContainsKey(student.ScheduleId))
                    {
                        result.Add(student.ScheduleId, student.ActivityId);
                    }
                }
            }

            return AppResult<IDictionary<int,int>>.CreateSucceeded(result, "Sucess");
        }
        catch (Exception ex)
        {
            return AppResult<IDictionary<int,int>>.CreateFailed(ex, "An error occured when getting required ids");
        }
    }
}