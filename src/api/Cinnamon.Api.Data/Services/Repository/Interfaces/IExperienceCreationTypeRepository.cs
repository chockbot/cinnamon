using Cinnamon.Framework.ApiCommand.ApiData.DTO.ExperienceCreationType;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces
{
    public interface IExperienceCreationTypeRepository
    {
        Task<AppResult<IEnumerable<ExperienceCreationTypeDTO>>> GetExperienceCreationTypes();
    }
}
