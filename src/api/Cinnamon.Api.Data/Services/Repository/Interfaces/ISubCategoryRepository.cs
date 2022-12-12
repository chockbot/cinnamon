using Cinnamon.Api.Data.Services.Repository.ExperienceSubCategory.DTO;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;
public interface ISubCategoryRepository
{
    Task<AppResult<SubCategoryDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<SubCategoryDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<SubCategoryDTO>>> GetAllAsync();
    Task<AppResult<SubCategoryDTO>> CreateSubCategoryAsync(int categoryId, string subcategory);
    Task<AppResult<SubCategoryDTO>> UpdateSubCategoryAsync(int subCategoryId, int? categoryId, string? subcategory);
}


