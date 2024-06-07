using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DirectStudentService;
public class CreateDirectStudentAttendanceHandler : ICreateDirectStudentAttendanceHandler
{
    private readonly IMapper mapper;
    private readonly IDirectStudentData directStudentData;
    public CreateDirectStudentAttendanceHandler(IDirectStudentData directStudentData, IMapper mapper)
    {
        this.directStudentData = directStudentData;
        this.mapper = mapper;
    }

    public AppResult<CreateDirectStudentAttendanceResult> Execute(CreateDirectStudentAttendanceArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<CreateDirectStudentAttendanceResult>> ExecuteAsync(CreateDirectStudentAttendanceArgs args)
    {
        try
        {
            List<Framework.ApiCommand.ApiData.DirectStudent.Request.CreateStudentAttendanceArgs.CreateStudentAttendance> studentAttendance = new();
            foreach (var student in args.CreateDirectStudentsAttendance)
            {
                studentAttendance.Add(new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateStudentAttendanceArgs.CreateStudentAttendance
                {
                    Date                   = student.AttendanceDate,
                    DirectStudentSessionId = student.StudentId,
                    IsPresent              = student.IsPresent
                });
            }
            var createDirect = await directStudentData.CreateStudentAttendance(new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateStudentAttendanceArgs
            {
                CreateStudentAttendances = studentAttendance
            });

            if (!createDirect.Succeeded || createDirect.Result == null || !createDirect.Result.IsSuccess)
            {
                return AppResult<CreateDirectStudentAttendanceResult>.CreateFailed(new ApplicationException(createDirect.Result?.ErrorInfo?.Message), createDirect.Message);
            }
            var directStudentRes = mapper.Map<IEnumerable<CreateDirectStudentAttendanceResult.CreateStudentAttendance>>(createDirect.Result.Result);
            return AppResult<CreateDirectStudentAttendanceResult>.CreateSucceeded(
            new CreateDirectStudentAttendanceResult{CreateDirectStudentsAttendance = directStudentRes}, "Successfully created");
        }
        catch (Exception ex)
        {
            return AppResult<CreateDirectStudentAttendanceResult>.CreateFailed(ex, "An error occurred in CreateDirectStudentAttendanceHandler");
        }
    }

}
