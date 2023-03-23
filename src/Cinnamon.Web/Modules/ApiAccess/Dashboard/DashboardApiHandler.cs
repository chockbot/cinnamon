using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.Dashboard;

public class DashboardApiHandler : IDashboardApiHandler
{
    private readonly IFlurlClient flurlClient;

    public DashboardApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
    {
        flurlClient = flurlFac.Get(config.ApiUrl);
    }

    public async Task<AppResult<CreateStudentAttendanceResult>> CreateStudentAttendance(CreateStudentAttendanceArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/CreateStudentAttendance")
                .PostJsonAsync(args)
                .ReceiveJson<CreateStudentAttendanceResult>();

            return AppResult<CreateStudentAttendanceResult>.CreateSucceeded(result, "Successfully posting create activity api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<CreateStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateStudentAttendanceResult>.CreateFailed(ex, "An error occured when posting create activity api");
        }
    }

    public async Task<AppResult<GetActivitySchedulesResult>> GetActivitySchedules(string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetActivitySchedules")
                .GetJsonAsync<GetActivitySchedulesResult>();

            return AppResult<GetActivitySchedulesResult>.CreateSucceeded(result, "Successfully getting experience types api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetActivitySchedulesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetActivitySchedulesResult>.CreateFailed(ex, "An error occured when getting experience types api");
        }
    }

    public async Task<AppResult<GetAllBadgesResult>> GetAllBadge()
    {
        try
        {
            var result = await flurlClient
                .Request("Dashboard/GetAllBadges")
                .GetJsonAsync<GetAllBadgesResult>();

            return AppResult<GetAllBadgesResult>.CreateSucceeded(result, "Successfully getting all ongoing activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllBadgesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllBadgesResult>.CreateFailed(ex, "An error occured when getting all ongoing activities api");
        }
    }

    public async Task<AppResult<GetAllStudentAttendanceByIdResult>> GetAllStudentAttendanceById(GetAllStudentAttendanceByIdArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetAllStudentAttendanceById")
                .SetQueryParams(args)
                .GetJsonAsync<GetAllStudentAttendanceByIdResult>();

            return AppResult<GetAllStudentAttendanceByIdResult>.CreateSucceeded(result, "Successfully getting experience types api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllStudentAttendanceByIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllStudentAttendanceByIdResult>.CreateFailed(ex, "An error occured when getting experience types api");
        }
    }

    public async Task<AppResult<GetCurrentAttendanceResult>> GetCurrentAttendance(GetCurrentAttendanceArgs args,string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetCurrentAttendance")
                .SetQueryParams(args)
                .GetJsonAsync<GetCurrentAttendanceResult>();

            return AppResult<GetCurrentAttendanceResult>.CreateSucceeded(result, "Successfully getting current attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCurrentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCurrentAttendanceResult>.CreateFailed(ex, "An error occured when getting student attendance api");
        }
    }

    public async Task<AppResult<GetStudentAttendanceResult>> GetStudentAttendance(GetStudentAttendanceArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetStudentAttendance")
                .SetQueryParams(args)
                .GetJsonAsync<GetStudentAttendanceResult>();

            return AppResult<GetStudentAttendanceResult>.CreateSucceeded(result, "Successfully getting student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetStudentAttendanceResult>.CreateFailed(ex, "An error occured when getting student attendance api");
        }
    }

    public async Task<AppResult<UpdateAttendanceResult>> UpdateAttendance(UpdateAttendanceArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .Request("Dashboard/UpdateAttendance")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateAttendanceResult>();

            return AppResult<UpdateAttendanceResult>.CreateSucceeded(result, "Successfully posting update attendance api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<UpdateAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateAttendanceResult>.CreateFailed(ex, "An error occured when posting update attendance api");
        }
    }

    public async Task<AppResult<UpdateStudentAttendanceResult>> UpdateStudentAttendances(UpdateStudentAttendnaceArgs args,string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/UpdateStudentAttendances")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateStudentAttendanceResult>();

            return AppResult<UpdateStudentAttendanceResult>.CreateSucceeded(result, "Successfully getting current attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateStudentAttendanceResult>.CreateFailed(ex, "An error occured when getting current attendance api");
        }
    }

}