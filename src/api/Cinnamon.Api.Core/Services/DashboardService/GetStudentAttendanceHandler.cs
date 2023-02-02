using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class GetStudentAttendanceHandler : IGetStudentAttendanceHandler
{
    private readonly IStudentAttendanceData studentAttendanceData;
    private readonly IGetOwnedActivityHandler getOwnedActivityHandler;
    private readonly IStudentData studentData;

    public GetStudentAttendanceHandler(IStudentAttendanceData studentAttendanceData, IGetOwnedActivityHandler getOwnedActivityHandler,
        IStudentData studentData)
    {
        this.studentAttendanceData = studentAttendanceData;
        this.getOwnedActivityHandler = getOwnedActivityHandler;
        this.studentData = studentData;
    }

    public AppResult<GetStudentAttendanceResult> Execute(GetStudentAttendanceArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetStudentAttendanceResult>.CreateFailed(ex, "An error occured in GetStudentAttendanceHandler");
        }
    }

    public async Task<AppResult<GetStudentAttendanceResult>> ExecuteAsync(GetStudentAttendanceArgs args)
    {
        try
        {
            var ownedActivityResult = await getOwnedActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetOwnedActivityArgs {
                ActivityId = args.ActivityId,
                IncludeAtivitySchedules = true
            });
            if(!ownedActivityResult.Succeeded || ownedActivityResult.Result == null)
            {
                return AppResult<GetStudentAttendanceResult>.CreateFailed(ownedActivityResult.Error.Exception, ownedActivityResult.Message);
            }
            var activity = ownedActivityResult.Result;

            // check schedule if associated to activity
            if(!activity.ActivitySchedules.Any(s => s.Id == args.ScheduleId))
            {
                return AppResult<GetStudentAttendanceResult>.CreateFailed(
                    new ApplicationException("Unable to determine activity and schedule"), "Unable to determine activity and schedule");
            }
            var schedule = activity.ActivitySchedules.First(s => s.Id == args.ScheduleId);

            // check if already have attendance schedule in the date provided
            // if not yet then create and insert to db.
            var attendanceRes = await studentAttendanceData.GetAllStudentAttendance(new Framework.ApiCommand.ApiData.StudentAttendance.Request.GetAllStudentAttendanceArgs {
                ActivityId = args.ActivityId,
                IsIncludeStudent = true,
                ScheduleId = args.ScheduleId,
                Date = args.Date.ToString("yyyyMMdd")
            });
            if(!attendanceRes.Succeeded || attendanceRes.Result == null || !attendanceRes.Result.IsSuccess)
            {
                return AppResult<GetStudentAttendanceResult>.CreateFailed(new ApplicationException(attendanceRes.Result?.ErrorInfo?.Message), attendanceRes.Message);
            }
            var attendances = attendanceRes.Result.Result;

            // don't have entries yet, then need to create attendance
            if(attendances.Count() == 0 && args.ForceCreate)
            {
                // get enrolled students ativity schedule
                var studentRes = await studentData.GetAllStudents(new Framework.ApiCommand.ApiData.Student.Request.GetAllStudentArgs {
                    ActivityId = args.ActivityId,
                    ScheduleId = args.ScheduleId,
                    Status = "ACTIVE"
                });
                if(!studentRes.Succeeded || studentRes.Result == null || !studentRes.Result.IsSuccess)
                {
                    return AppResult<GetStudentAttendanceResult>.CreateFailed(
                        new ApplicationException(studentRes.Result?.ErrorInfo?.Message), studentRes.Message);
                }
                var students = studentRes.Result.Result;

                // don't have enrolled students in the associated activity schedule
                // return success with empty students
                if(students.Count() == 0)
                {
                    return AppResult<GetStudentAttendanceResult>.CreateSucceeded(new GetStudentAttendanceResult {
                        StudentAttendaces = Enumerable.Empty<GetStudentAttendanceResult.StudentAttendace>()
                    }, "Don't heve yet students enrolled in the specified activity schedule");
                }

                // create student attendance
                DateTime date = DateTime.Now.Date;
                var studentsToCreate = students.Select(s => {
                    return new Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request.CreateManyStudentAttendanceArgs.StudentAttendaceDetails {
                        Date = date,
                        IsPresent = false,
                        StudentId = s.Id
                    };
                });

                var createStudentAttendance = await studentAttendanceData.CreateManyStudentAttendance(new Framework.ApiCommand.ApiData.StudentAttendance.Request.CreateManyStudentAttendanceArgs {
                    StudentAttendaces = studentsToCreate
                });
                if(!createStudentAttendance.Succeeded || createStudentAttendance.Result == null || !createStudentAttendance.Result.IsSuccess)
                {
                    return AppResult<GetStudentAttendanceResult>.CreateFailed(
                        new ApplicationException(createStudentAttendance.Result?.ErrorInfo?.Message), createStudentAttendance.Message);
                }

                // fetch again student attendance
                var attendanceResReLoad = await studentAttendanceData.GetAllStudentAttendance(new Framework.ApiCommand.ApiData.StudentAttendance.Request.GetAllStudentAttendanceArgs {
                    ActivityId = args.ActivityId,
                    IsIncludeStudent = true,
                    ScheduleId = args.ScheduleId,
                    Date = args.Date.ToString("yyyyMMdd")
                });
                if(!attendanceResReLoad.Succeeded || attendanceResReLoad.Result == null || !attendanceResReLoad.Result.IsSuccess)
                {
                    return AppResult<GetStudentAttendanceResult>.CreateFailed(new ApplicationException(attendanceResReLoad.Result?.ErrorInfo?.Message), attendanceResReLoad.Message);
                }

                return AppResult<GetStudentAttendanceResult>.CreateSucceeded(new GetStudentAttendanceResult {
                    StudentAttendaces = attendanceResReLoad.Result.Result.Select(s => {
                        return new GetStudentAttendanceResult.StudentAttendace {
                            ActivityDescription = activity.Description,
                            ActivityId = activity.Id,
                            ActivityTitle = activity.Title,
                            IsPresent = s.IsPresent,
                            ScheduleDescription = schedule.Name,
                            ScheduleId = schedule.Id,
                            ScheduleTitle = schedule.DateTime,
                            StudentId = s.StudentId,
                            NumberOfSessions = s.Student.NumberOfSessions,
                            SessionsAttended = s.Student.SessionsAttended,
                            Status = s.Student.Status,
                            StudentName = s.Student.Name,
                            StudentNo = s.Student.StudentNo,
                            AttendanceDate = s.Date,
                            Id = s.Id,
                            Remarks = s.Student.Remarks
                        };
                    })
                }, "Successfullt get student attendance");
            }

            return AppResult<GetStudentAttendanceResult>.CreateSucceeded(new GetStudentAttendanceResult {
                StudentAttendaces = attendances.Select(s => {
                    return new GetStudentAttendanceResult.StudentAttendace {
                        ActivityDescription = activity.Description,
                        ActivityId = activity.Id,
                        ActivityTitle = activity.Title,
                        IsPresent = s.IsPresent,
                        ScheduleDescription = schedule.Name,
                        ScheduleId = schedule.Id,
                        ScheduleTitle = schedule.DateTime,
                        StudentId = s.StudentId
                    };
                })
            }, "Successfullt get student attendance");
        }
        catch (Exception ex)
        {
            return AppResult<GetStudentAttendanceResult>.CreateFailed(ex, "An error occured in GetStudentAttendanceHandler");
        }
    }
}