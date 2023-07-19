using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Response;
using Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;
using Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.StudentAttendance;

public class StudentAttendanceData : IStudentAttendanceData
{
    private readonly IFlurlClient flurlClient;

	public StudentAttendanceData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }
    
    public async Task<AppResult<CreateManyStudentAttendanceResult>> CreateManyStudentAttendance(CreateManyStudentAttendanceArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("StudentAttendance/CreateManyStudentAttendance")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateManyStudentAttendanceResult>();

            return AppResult<CreateManyStudentAttendanceResult>.CreateSucceeded(result, "Successfully posting create many student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateManyStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateManyStudentAttendanceResult>.CreateFailed(ex, "An error occured when posting create many student attendance api");
        }
    }

    public async Task<AppResult<CreateStudentAttendanceResult>> CreateStudentAttendance(CreateStudentAttendaceArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("StudentAttendance/CreateStudentAttendance")
                .PostJsonAsync(args)
                .ReceiveJson<CreateStudentAttendanceResult>();

            return AppResult<CreateStudentAttendanceResult>.CreateSucceeded(result, "Successfully posting create student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateStudentAttendanceResult>.CreateFailed(ex, "An error occured when posting create student attendance api");
        }
    }

    public async Task<AppResult<GetAllStudentAttendanceResult>> GetAllStudentAttendance(GetAllStudentAttendanceArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("StudentAttendance/GetAllStudentAttendance")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllStudentAttendanceResult>();

            return AppResult<GetAllStudentAttendanceResult>.CreateSucceeded(result, "Successfully getting get all student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllStudentAttendanceResult>.CreateFailed(ex, "An error occured when getting all student attendance api");
        }
    }

    public async Task<AppResult<GetStudentAttendanceResult>> GetStudentAttendanceById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"StudentAttendance/GetStudentAttendanceById/{id}")
                            .GetJsonAsync<GetStudentAttendanceResult>();

            return AppResult<GetStudentAttendanceResult>.CreateSucceeded(result, "Successfully getting student attendance by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetStudentAttendanceResult>.CreateFailed(ex, "An error occured when getting student attendance by id api");
        }
    }

    public async Task<AppResult<UpdateManyStudentAttendanceResult>> UpdateManyStudentAttendance(UpdateManyStudentAttendanceArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("StudentAttendance/UpdateManyStudentAttendance")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateManyStudentAttendanceResult>();

            return AppResult<UpdateManyStudentAttendanceResult>.CreateSucceeded(result, "Successfully posting update many student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateManyStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateManyStudentAttendanceResult>.CreateFailed(ex, "An error occured when posting update many student attendance api");
        }
    }

    public async Task<AppResult<UpdateStudentAttendanceResult>> UpdateStudentAttendance(UpdateStudentAttendanceArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("StudentAttendance/UpdateStudentAttendance")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateStudentAttendanceResult>();

            return AppResult<UpdateStudentAttendanceResult>.CreateSucceeded(result, "Successfully posting update student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateStudentAttendanceResult>.CreateFailed(ex, "An error occured when posting update student attendance api");
        }
    }

    public async Task<AppResult<UpdateAttendanceResult>> UpdateAttendance(UpdateAttendanceArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("StudentAttendance/UpdateAttendance")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateAttendanceResult>();

            return AppResult<UpdateAttendanceResult>.CreateSucceeded(result, "Successfully posting update student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateAttendanceResult>.CreateFailed(ex, "An error occured when posting update student attendance api");
        }
    }

    public async Task<AppResult<GetAllStudentAttendanceByIdResult>> GetAllStudentAttendanceById(GetAllStudentAttendanceByIdArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("StudentAttendance/GetAllStudentAttendanceById")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllStudentAttendanceByIdResult>();

            return AppResult<GetAllStudentAttendanceByIdResult>.CreateSucceeded(result, "Successfully getting get all student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllStudentAttendanceByIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllStudentAttendanceByIdResult>.CreateFailed(ex, "An error occured when getting all completed student attendance api");
        }
    }

    public async Task<AppResult<GetCompletedStudentsResult>> GetCompletedStudents(GetCompletedStudentsArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("StudentAttendance/GetCompletedStudents")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetCompletedStudentsResult>();

            return AppResult<GetCompletedStudentsResult>.CreateSucceeded(result, "Successfully getting get all completed student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCompletedStudentsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCompletedStudentsResult>.CreateFailed(ex, "An error occured when getting student last attendance api");
        }
    }

    public async Task<AppResult<GetStudentLastAttendanceResult>> GetStudentLastAttendance(GetStudentLastAttendanceArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("StudentAttendance/GetStudentLastAttendance")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetStudentLastAttendanceResult>();

            return AppResult<GetStudentLastAttendanceResult>.CreateSucceeded(result, "Successfully getting student last attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetStudentLastAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetStudentLastAttendanceResult>.CreateFailed(ex, "An error occured when getting student last attendance api");
        }
    }
}