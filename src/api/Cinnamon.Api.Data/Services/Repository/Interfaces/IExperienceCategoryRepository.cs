using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Services.Repository.ExperienceCategory.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Cinnamon.Api.Data.Services.Repository.Activity.DTO;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IExperienceCategoryRepository
{
    Task<AppResult<ExperienceCategoryDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<ExperienceCategoryDTO>>> GetAllAsync();
    Task<AppResult<ExperienceCategoryDTO>> CreateExperienceCategoryAsync(string category, string iconPath);
    Task<AppResult<ExperienceCategoryDTO>> UpdateExperienceCategoryAsync(int id, string? category, string? iconPath);
}

