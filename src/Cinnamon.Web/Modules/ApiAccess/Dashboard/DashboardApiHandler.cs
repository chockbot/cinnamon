using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Response;
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
    public async Task<AppResult<GetAllStudentAttendanceResult>> GetAllStudentsAttendance(GetAllStudentAttendanceArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetAllStudentsAttendance")
                .SetQueryParams(args)
                .GetJsonAsync<GetAllStudentAttendanceResult>();

            return AppResult<GetAllStudentAttendanceResult>.CreateSucceeded(result, "Successfully getting student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllStudentAttendanceResult>.CreateFailed(ex, "An error occured when getting student attendance api");
        }
    }
    public async Task<AppResult<GetCompletedStudentsResult>> GetCompletedStudents(GetCompletedStudetnsArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetCompletedStudents")
                .SetQueryParams(args)
                .GetJsonAsync<GetCompletedStudentsResult>();

            return AppResult<GetCompletedStudentsResult>.CreateSucceeded(result, "Successfully getting student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetCompletedStudentsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetCompletedStudentsResult>.CreateFailed(ex, "An error occured when getting student attendance api");
        }
    }

    public async Task<AppResult<GetOTEByProviderResult>> GetOTEByProvider(GetOTEByProviderArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetOTEByProvider")
                .SetQueryParams(args)
                .GetJsonAsync<GetOTEByProviderResult>();

            return AppResult<GetOTEByProviderResult>.CreateSucceeded(result, "Successfully getting ote activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetOTEByProviderResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetOTEByProviderResult>.CreateFailed(ex, "An error occured when getting ote activities api");
        }
    }

    public async Task<AppResult<GetOTEByActivityIdResult>> GetOTEByActivityId(GetOTEByActivityIdArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetOTEByActivityId")
                .SetQueryParams(args)
                .GetJsonAsync<GetOTEByActivityIdResult>();

            return AppResult<GetOTEByActivityIdResult>.CreateSucceeded(result, "Successfully getting ote activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetOTEByActivityIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetOTEByActivityIdResult>.CreateFailed(ex, "An error occured when getting ote activities api");
        }
    }

    public async Task<AppResult<GetTicketDetailsResult>> GetTicketDetails(GetTicketDetailsArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetTicketDetails")
                .SetQueryParams(args)
                .GetJsonAsync<GetTicketDetailsResult>();

            return AppResult<GetTicketDetailsResult>.CreateSucceeded(result, "Successfully getting ticket details api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetTicketDetailsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetTicketDetailsResult>.CreateFailed(ex, "An error occured when getting ticket details api");
        }
    }

    public async Task<AppResult<UpdateOTETicketResult>> UpdateOTETicket(UpdateOTETicketArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .Request("Dashboard/UpdateOTETicket")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateOTETicketResult>();

            return AppResult<UpdateOTETicketResult>.CreateSucceeded(result, "Successfully posting update ote ticket");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<UpdateOTETicketResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateOTETicketResult>.CreateFailed(ex, "An error occured when posting update ote ticket");
        }
    }

    public async Task<AppResult<GetDisbursementByProviderResult>> GetDisbursementByProvider(GetDisbursementByProviderArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetDisbursementByProvider")
                .SetQueryParams(args)
                .GetJsonAsync<GetDisbursementByProviderResult>();

            return AppResult<GetDisbursementByProviderResult>.CreateSucceeded(result, "Successfully getting disbursement details api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetDisbursementByProviderResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetDisbursementByProviderResult>.CreateFailed(ex, "An error occurred when getting disbursement details api");
        }
    }

    public async Task<AppResult<GetEnrolledStudentsByProviderResult>> GetEnrolledStudentsByProvider(GetEnrolledStudentsByProviderArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/GetEnrolledStudentsByProvider")
                .SetQueryParams(args)
                .GetJsonAsync<GetEnrolledStudentsByProviderResult>();

            return AppResult<GetEnrolledStudentsByProviderResult>.CreateSucceeded(result, "Successfully getting enrolled students api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetEnrolledStudentsByProviderResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetEnrolledStudentsByProviderResult>.CreateFailed(ex, "An error occurred when getting enrolled students api");
        }
    }

    public async Task<AppResult<CreateDirectStudentsResult>> CreateDirectStudents(CreateDirectStudentsArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("Dashboard/CreateDirectStudents")
                .PostJsonAsync(args)
                .ReceiveJson<CreateDirectStudentsResult>();

            return AppResult<CreateDirectStudentsResult>.CreateSucceeded(result, "Successfully posting create direct students api");
        }
        catch (FlurlHttpException ex)
        {
            var error = await ex.GetResponseJsonAsync();
            return AppResult<CreateDirectStudentsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateDirectStudentsResult>.CreateFailed(ex, "An error occured when posting create direct students api");
        }
    }
}