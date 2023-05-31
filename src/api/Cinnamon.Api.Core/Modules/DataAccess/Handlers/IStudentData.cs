using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IStudentData
{
    Task<AppResult<GetStudentResult>> GetStudentById(int Id, int activityId);
    Task<AppResult<GetAllStudentResult>> GetAllStudents(GetAllStudentArgs args);
    Task<AppResult<GetEnrolledStudentsResult>> GetEnrolledStudents(int activityId);
    Task<AppResult<CreateStudentResult>> CreateStudent(CreateStudentArgs args);
    Task<AppResult<CreateManyStudentResult>> CreateManyStudent(CreateManyStudentArgs args);
    Task<AppResult<UpdateStudentResult>> UpdateStudent(UpdateStudentArgs args);
    Task<AppResult<GetStudentsToDisburseResult>> GetStudentsToDisburse(GetStudentsToDisburseArgs args);
    Task<AppResult<UpdateStudentDisbursementStatusResult>> UpdateStudentsDisbursementStatus(UpdateStudentDisbursementArgs args);
}
