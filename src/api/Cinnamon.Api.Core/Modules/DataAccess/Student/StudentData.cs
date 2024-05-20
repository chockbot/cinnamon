using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Student;

public class StudentData: IStudentData
{
	private readonly IFlurlClient flurlClient;

	public StudentData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
		flurlClient = flurlFac.Get(config.ApiDataUrl);
	}

	public async Task<AppResult<CreateStudentResult>> CreateStudent(CreateStudentArgs args)
	{
		try
		{
			var result = await flurlClient
				.Request("Student/CreateStudent")
				.PostJsonAsync(args)
				.ReceiveJson<CreateStudentResult>();

			return AppResult<CreateStudentResult>.CreateSucceeded(result, "Successfully posting create student api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<CreateStudentResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<CreateStudentResult>.CreateFailed(ex, "An error occured when posting create student api");
		}
	}
	
	public async Task<AppResult<GetStudentResult>> GetStudentById(int Id, int activityId)
	{
		try
		{
			var result = await flurlClient
							.Request($"Student/GetStudentById/{Id}")
							.SetQueryParams(activityId)
							.GetJsonAsync<GetStudentResult>();

			return AppResult<GetStudentResult>.CreateSucceeded(result, "Successfully getting student by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetStudentResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetStudentResult>.CreateFailed(ex, "An error occured when getting student by id api");
		}
	}

	public async Task<AppResult<GetAllStudentResult>> GetAllStudents(GetAllStudentArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Student/GetAllStudents")
							.SetQueryParams(args)
							.GetJsonAsync<GetAllStudentResult>();

			return AppResult<GetAllStudentResult>.CreateSucceeded(result, "Successfully getting get all students api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetAllStudentResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetAllStudentResult>.CreateFailed(ex, "An error occured when getting all students api");
		}
	}

	public async Task<AppResult<UpdateStudentResult>> UpdateStudent(UpdateStudentArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Student/UpdateStudent")
							.PostJsonAsync(args)
							.ReceiveJson<UpdateStudentResult>();

			return AppResult<UpdateStudentResult>.CreateSucceeded(result, "Successfully posting update student api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<UpdateStudentResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<UpdateStudentResult>.CreateFailed(ex, "An error occured when posting update student api");
		}
	}

	public async Task<AppResult<CreateManyStudentResult>> CreateManyStudent(CreateManyStudentArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Student/CreateManyStudent")
							.PostJsonAsync(args)
							.ReceiveJson<CreateManyStudentResult>();

			return AppResult<CreateManyStudentResult>.CreateSucceeded(result, "Successfully posting create many student api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<CreateManyStudentResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<CreateManyStudentResult>.CreateFailed(ex, "An error occured when posting create many student api");
		}
	}

	public async Task<AppResult<GetEnrolledStudentsResult>> GetEnrolledStudents(int activityId)
	{
		try
		{
			var result = await flurlClient
							.Request($"Student/GetEnrolledStudents/{activityId}")
							.GetJsonAsync<GetEnrolledStudentsResult>();

			return AppResult<GetEnrolledStudentsResult>.CreateSucceeded(result, "Successfully getting student by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetEnrolledStudentsResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetEnrolledStudentsResult>.CreateFailed(ex, "An error occured when getting student by id api");
		}
	}

	public async Task<AppResult<GetStudentsToDisburseResult>> GetStudentsToDisburse(GetStudentsToDisburseArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Student/GetStudentsToDisburse")
							.SetQueryParams(args)
							.GetJsonAsync<GetStudentsToDisburseResult>();

			return AppResult<GetStudentsToDisburseResult>.CreateSucceeded(result, "Successfully getting get all students api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetStudentsToDisburseResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetStudentsToDisburseResult>.CreateFailed(ex, "An error occured when getting all students api");
		}
	}

	public async Task<AppResult<UpdateStudentDisbursementStatusResult>> UpdateStudentsDisbursementStatus(UpdateStudentDisbursementArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Student/UpdateStudentsDisbursementStatus")
							.PostJsonAsync(args)
							.ReceiveJson<UpdateStudentDisbursementStatusResult>();

			return AppResult<UpdateStudentDisbursementStatusResult>.CreateSucceeded(result, "Successfully posting update student disbursement status");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<UpdateStudentDisbursementStatusResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<UpdateStudentDisbursementStatusResult>.CreateFailed(ex, "An error occured when posting update student disbursement status");
		}
	}
	public async Task<AppResult<GetCompletedStudentsByIdResult>> GetCompletedStudentsById(GetCompletedStudentsByIdArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Student/GetCompletedStudentsById")
							.SetQueryParams(args)
							.GetJsonAsync<GetCompletedStudentsByIdResult>();

			return AppResult<GetCompletedStudentsByIdResult>.CreateSucceeded(result, "Successfully getting get all completed student attendance by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetCompletedStudentsByIdResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetCompletedStudentsByIdResult>.CreateFailed(ex, "An error occured when getting all completed student attendance by id api");
		}
	}

	public async Task<AppResult<GetAllStudentsByIdResult>> GetAllStudentsById(GetAllStudentsByIdArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Student/GetAllStudentsById")
							.SetQueryParams(args)
							.GetJsonAsync<GetAllStudentsByIdResult>();

			return AppResult<GetAllStudentsByIdResult>.CreateSucceeded(result, "Successfully getting get all student attendance by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetAllStudentsByIdResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetAllStudentsByIdResult>.CreateFailed(ex, "An error occured when getting all student attendance by id api");
		}
	}

	public async Task<AppResult<GetExpiringStudentsResult>> GetExpiringStudents()
	{
		try
		{
			var result = await flurlClient
							.Request("Student/GetExpiringStudents")
							.GetJsonAsync<GetExpiringStudentsResult>();

			return AppResult<GetExpiringStudentsResult>.CreateSucceeded(result, "Successfully getting get all expiring students");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetExpiringStudentsResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetExpiringStudentsResult>.CreateFailed(ex, "An error occurred when getting all expiring students");
		}
	}

	public async Task<AppResult<GetEnrolleeMasterListResult>> GetEnrolleeMasters(GetEnrolleeMasterListArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Student/GetEnrolleeMasterList")
							.SetQueryParams(args)
							.GetJsonAsync<GetEnrolleeMasterListResult>();

			return AppResult<GetEnrolleeMasterListResult>.CreateSucceeded(result, "Successfully getting get enrollee master list by provider id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetEnrolleeMasterListResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetEnrolleeMasterListResult>.CreateFailed(ex, "An error occurred when getting enrollee master list by provider id api");
		}
	}

    public async Task<AppResult<GetEnrolledStudentsResult>> GetEnrolledStudentsByProvider(GetEnrolledStudentsArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Student/GetEnrolledStudentsByProvider")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetEnrolledStudentsResult>();

            return AppResult<GetEnrolledStudentsResult>.CreateSucceeded(result, "Successfully getting get enrolled students list by provider id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetEnrolledStudentsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetEnrolledStudentsResult>.CreateFailed(ex, "An error occurred when getting enrolled students by provider id api");
        }
    }
}
