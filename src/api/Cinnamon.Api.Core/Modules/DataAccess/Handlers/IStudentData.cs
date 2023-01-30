using Cinnamon.Framework.ApiCommand.ApiData.Student.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IStudentData
{
    Task<AppResult<GetStudentResult>> GetStudentById(int id);
    Task<AppResult<GetAllStudentResult>> GetAllStudents(GetAllStudentArgs args);
    Task<AppResult<CreateStudentResult>> CreateStudent(CreateStudentArgs args);
    Task<AppResult<UpdateStudentResult>> UpdateStudent(UpdateStudentArgs args);
}
