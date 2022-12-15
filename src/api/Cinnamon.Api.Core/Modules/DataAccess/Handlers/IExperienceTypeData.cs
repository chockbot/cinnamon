using Cinnamon.Framework.ApiCommand.ApiData.ExperienceType.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceType.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;
public interface IExperienceTypeData
{
    Task<AppResult<GetExperienceTypeResult>> GetExperienceTypeById(int id);
    Task<AppResult<GetAllExperienceTypeResult>> GetAllExperienceType();
    Task<AppResult<CreateExperienceTypeResult>> CreateExperienceType(string name);
    Task<AppResult<UpdateExperienceTypeResult>> UpdateExperienceType(UpdateExperienceTypeArgs args);
}
