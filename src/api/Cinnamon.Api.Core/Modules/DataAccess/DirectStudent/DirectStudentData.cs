using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.DirectStudent;

public class DirectStudentData : IDirectStudentData
{
    private readonly IFlurlClient flurlClient;

    public DirectStudentData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateDirectStudentsResult>> CreateDirectStudents(CreateDirectStudentsArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("DirectStudent")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateDirectStudentsResult>();
            return AppResult<CreateDirectStudentsResult>.CreateSucceeded(result, "Successfully posting create direct students api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<CreateDirectStudentsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateDirectStudentsResult>.CreateFailed(ex, "An error occured when posting create direct students api");
        }
    }

    public async Task<AppResult<GetDirectStudentsResult>> GetDirectStudents(GetDirectStudentsArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("DirectStudent")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetDirectStudentsResult>();
            return AppResult<GetDirectStudentsResult>.CreateSucceeded(result, "Successfully get direct students api.");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<GetDirectStudentsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetDirectStudentsResult>.CreateFailed(ex, "An error occured when get direct students api.");
        }
    }

    public async Task<AppResult<StudentAttendanceResult>> StudentAttendance(StudentAttendanceArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("DirectStudent/Attendance")
                            .SetQueryParams(args)
                            .GetJsonAsync<StudentAttendanceResult>();
            return AppResult<StudentAttendanceResult>.CreateSucceeded(result, "Successfully get direct student attendance api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<StudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<StudentAttendanceResult>.CreateFailed(ex, "An error occured when get direct student attendance api");
        }
    }

    public async Task<AppResult<CreateDirectStudentAttendanceResult>> CreateStudentAttendance(CreateStudentAttendanceArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("DirectStudent/Attendance/Bulk")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateDirectStudentAttendanceResult>();
            return AppResult<CreateDirectStudentAttendanceResult>.CreateSucceeded(result, "Successfully posting create direct students attendance api.");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<CreateDirectStudentAttendanceResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateDirectStudentAttendanceResult>.CreateFailed(ex, "An error occured when posting create direct students attendance api.");
        }
    }

    public async Task<AppResult<UpdateStudentAttendanceBulkResult>> UpdateStudentAttendance(UpdateStudentAttendanceBulkArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("DirectStudent/Attendance/Update/Bulk")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateStudentAttendanceBulkResult>();
            return AppResult<UpdateStudentAttendanceBulkResult>.CreateSucceeded(result, "Successfully posting update direct students attendance api.");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<UpdateStudentAttendanceBulkResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateStudentAttendanceBulkResult>.CreateFailed(ex, "An error occured when posting update direct students attendance api.");
        }
    }

    public async Task<AppResult<StudentInfosResult>> StudentInfos(StudentInfosArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("DirectStudent/Infos")
                            .SetQueryParams(args)
                            .GetJsonAsync<StudentInfosResult>();
            return AppResult<StudentInfosResult>.CreateSucceeded(result, "Successfully get direct student infos api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<StudentInfosResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<StudentInfosResult>.CreateFailed(ex, "An error occured when get direct student infos api");
        }
    }

    public async Task<AppResult<GetDirectStudentsPaymentResult>> GetDirectStudentsPayments(GetDirectStudentsPaymentArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("DirectStudent/GetDirectStudentsPayment")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetDirectStudentsPaymentResult>();
            return AppResult<GetDirectStudentsPaymentResult>.CreateSucceeded(result, "Successfully get direct students api.");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<GetDirectStudentsPaymentResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetDirectStudentsPaymentResult>.CreateFailed(ex, "An error occured when get direct students api.");
        }
    }

    public async Task<AppResult<UpdateDirectStudentResult>> UpdateDirectStudent(UpdateDirectStudentArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request($"DirectStudent/UpdateDirectStudent")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateDirectStudentResult>();
            return AppResult<UpdateDirectStudentResult>.CreateSucceeded(result, "Successfully posting update direct students api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<UpdateDirectStudentResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateDirectStudentResult>.CreateFailed(ex, "An error occured when posting update direct students api");
        }
    }

    public async Task<AppResult<DirectStudentInfoResult>> DirectStudentInfo(int studentId)
    {
        try
        {
            var result = await flurlClient
                            .Request($"DirectStudent/Infos/{studentId}")
                            .GetJsonAsync<DirectStudentInfoResult>();
            return AppResult<DirectStudentInfoResult>.CreateSucceeded(result, "Successfully get direct student info.");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<DirectStudentInfoResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DirectStudentInfoResult>.CreateFailed(ex, "An error occured when get direct student info.");
        }
    }

    public async Task<AppResult<GetDirectStudentByIdResult>> GetDirectStudentsById(GetDirectStudentByIdArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request($"DirectStudent/GetStudentAttendanceById")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetDirectStudentByIdResult>();
            return AppResult<GetDirectStudentByIdResult>.CreateSucceeded(result, "Successfully get direct students api.");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<GetDirectStudentByIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetDirectStudentByIdResult>.CreateFailed(ex, "An error occured when get direct students api.");
        }
    }
}