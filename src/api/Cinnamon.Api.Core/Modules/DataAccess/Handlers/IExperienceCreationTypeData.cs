using Cinnamon.Framework.ApiCommand.ApiData.ExperienceCreationType.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ExperienceCreationType.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers
{
    public interface IExperienceCreationTypeData
    {
        Task<AppResult<GetExperienceCreationTypeResult>> GetExperienceCreationTypes(GetExperienceCreationTypeArgs args);
    }
}
