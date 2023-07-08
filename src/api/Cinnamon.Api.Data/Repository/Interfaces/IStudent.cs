using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IStudent : IGenericEntity<Student>
{
    Task<AppResult<IEnumerable<DisburseStudentDTO>>> GetAllStudentsToDisburse();
    Task<AppResult<IEnumerable<Student>>> UpdateStudentsDisbursementStatus(IEnumerable<Student> students);
    Task<AppResult<IEnumerable<DisburseStudentDTO>>> GetAllInclusiveStudentsToDisburse();
}