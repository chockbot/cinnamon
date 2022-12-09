using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Services.Repository.ExperienceCategory.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IExperienceCategoryRepository
{
    Task<AppResult<ExperienceCategoryDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<ExperienceCategoryDTO>>> GetAllAsync();
    Task<AppResult<ExperienceCategoryDTO>> SaveDateAsync(ExperienceCategoryDTO model);
}

