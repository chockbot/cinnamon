using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IDirectStudentInfo : IGenericEntity<DirectStudentInfo>
{
    Task<AppResult<IEnumerable<DirectStudentDTO>>> CreateDirectStudents(IEnumerable<DirectStudentDTO> directStudents);

    Task<AppResult<DirectStudentInfo>> UpdateDirectStudent(DirectStudentInfo? directStudent, IEnumerable<DirectStudentSession>? sessions);
    
    Task<AppResult<DirectStudentDTO>> DirecStudentInfo(int studentId);
}