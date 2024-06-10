using Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.DirectStudent;

public class DirectStudentApiHandler : IDirectStudentApiHandler
{
    private readonly IFlurlClient flurlClient;

    public DirectStudentApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
    {
        flurlClient = flurlFac.Get(config.ApiUrl);
    }

    public async Task<AppResult<DirectStudentInfoReult>> DirectStudents(DirectStudentInfoArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("DirectStudents")
                .SetQueryParams(args)
                .GetJsonAsync<DirectStudentInfoReult>();

            return AppResult<DirectStudentInfoReult>.CreateSucceeded(result, "Successfully getting direct students api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<DirectStudentInfoReult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DirectStudentInfoReult>.CreateFailed(ex, "An error occured when getting direct students api");
        }
    }

    public async Task<AppResult<UpdateStudentResult>> UpdateStudent(UpdateStudentArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"DirectStudents/UpdateStudent")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateStudentResult>();

            return AppResult<UpdateStudentResult>.CreateSucceeded(result, "Successfully update direct students api");
        }
        catch (FlurlHttpException ex)
        {
            var errorResult = await ex.GetResponseJsonAsync();
            return AppResult<UpdateStudentResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateStudentResult>.CreateFailed(ex, "An error occured when update direct students api");
        }
    }

    public async Task<AppResult<DirectStudentResult>> DirectStudent(int studentId, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"DirectStudents/{studentId}")
                .GetJsonAsync<DirectStudentResult>();

            return AppResult<DirectStudentResult>.CreateSucceeded(result, "Successfully getting direct student api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<DirectStudentResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DirectStudentResult>.CreateFailed(ex, "An error occured when getting direct student api");
        }
    }

    public async Task<AppResult<StudentSessionsResult>> StudentSessions(StudentSessionsArgs args, int studentId, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"DirectStudents/{studentId}/Sessions")
                .SetQueryParams(args)
                .GetJsonAsync<StudentSessionsResult>();

            return AppResult<StudentSessionsResult>.CreateSucceeded(result, "Successfully getting direct student sessions.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<StudentSessionsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<StudentSessionsResult>.CreateFailed(ex, "An error occured when getting direct student sessions.");
        }
    }

    public async Task<AppResult<GetDirectStudentByIdResult>> GetDirectStudentById(GetDirectStudentByIdArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"DirectStudents/GetStudentAttendanceById")
                .SetQueryParams(args)
                .GetJsonAsync<GetDirectStudentByIdResult>();

            return AppResult<GetDirectStudentByIdResult>.CreateSucceeded(result, "Successfully getting direct students api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetDirectStudentByIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetDirectStudentByIdResult>.CreateFailed(ex, "An error occured when getting direct students api");
        }
    }

    public async Task<AppResult<CreateDirectStudentAttendanceResult>> CreateDirectStudentAttendance(CreateDirectStudentAttendanceArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"DirectStudents/CreateDirectAttendance")
                .PostJsonAsync(args)
                .ReceiveJson<CreateDirectStudentAttendanceResult>();

            return AppResult<CreateDirectStudentAttendanceResult>.CreateSucceeded(result, "Successfully created direct student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            var errorResult = await ex.GetResponseJsonAsync();
            return AppResult<CreateDirectStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateDirectStudentAttendanceResult>.CreateFailed(ex, "An error occured when creating direct student attendance api");
        }
    }

    public async Task<AppResult<UpdateDirectStudentAttendanceResult>> UpdateDirectStudentAttendance(UpdateDirectStudentAttendanceArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"DirectStudents/UpdateDirectAttendance")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateDirectStudentAttendanceResult>();

            return AppResult<UpdateDirectStudentAttendanceResult>.CreateSucceeded(result, "Successfully updated direct student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            var errorResult = await ex.GetResponseJsonAsync();
            return AppResult<UpdateDirectStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateDirectStudentAttendanceResult>.CreateFailed(ex, "An error occured when updating direct student attendance api");
        }
    }
}