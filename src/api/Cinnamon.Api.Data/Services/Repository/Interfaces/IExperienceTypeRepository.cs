using Cinnamon.Api.Data.Services.Repository.ActivityDescription.DTO;
using Cinnamon.Api.Data.Services.Repository.ExperienceType.DTO;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces
{
    public interface IExperienceTypeRepository
    {
        Task<AppResult<ExperienceTypeDTO>> GetByIdAsync(int id);
        Task<AppResult<IEnumerable<ExperienceTypeDTO>>> GetAllAsync();
        Task<AppResult<ExperienceTypeDTO>> UpdateExperienceType(int Id, string name);
        Task<AppResult<ExperienceTypeDTO>> CreateExperienceType(string name);
    }
}
