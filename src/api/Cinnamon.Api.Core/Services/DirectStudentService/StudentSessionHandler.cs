using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DirectStudentService;

public class StudentSessionHandler : IStudentSessionHandler
{
    private readonly IDirectStudentData directStudentData;
    private readonly IDirectStudentHandler directStudentHandler;
    private readonly IMapper mapper;

    public StudentSessionHandler(IDirectStudentData directStudentData, IDirectStudentHandler directStudentHandler,
        IMapper mapper)
    {
        this.directStudentData = directStudentData;
        this.directStudentHandler = directStudentHandler;
        this.mapper = mapper;
    }
    
    public AppResult<StudentSessionResult> Execute(StudentSessionArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<StudentSessionResult>> ExecuteAsync(StudentSessionArgs args)
    {
        try
        {
            var sessionRes = await directStudentData.StudentSession(args.SessionId);
            if(!sessionRes.Succeeded || sessionRes.Result is null || !sessionRes.Result.IsSuccess)
            {
                return AppResult<StudentSessionResult>.CreateFailed(new ApplicationException(sessionRes.Result?.ErrorInfo?.Message), sessionRes.Message);
            }

            var directStudentRes = await directStudentHandler.ExecuteAsync(new DirectStudentArgs {
                StudentId = sessionRes.Result.Result.DirectStudentInfoId
            });
            if(!directStudentRes.Succeeded || directStudentRes.Result is null)
            {
                return AppResult<StudentSessionResult>.CreateFailed(new ApplicationException(directStudentRes.Message), directStudentRes.Message);
            }

            var result = mapper.Map<StudentSessionResult>(sessionRes.Result.Result);

            return AppResult<StudentSessionResult>.CreateSucceeded(result, "Successfully get student session,");
        }
        catch (System.Exception ex)
        {
            return AppResult<StudentSessionResult>.CreateFailed(ex, "An error occured in StudentSessionHandler.");
        }
    }
}