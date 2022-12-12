using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.ExperienceCategory.DTO;
using Cinnamon.Api.Data.Services.Repository.ExperienceSubCategory.DTO;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.ExperienceSubCategory;
public class SubCategoryRepository: ISubCategoryRepository
{
    private readonly IDataStore dataStore;
	public SubCategoryRepository(IDataStore dataStore)
	{
        this.dataStore = dataStore;
    }
    public async Task<AppResult<SubCategoryDTO>> CreateSubCategoryAsync(int categoryId, string subCategory)
    {
		try
		{
			var subCategoryCheck = await dataStore.SubCategory.FindFirstAsync(w => w.SubCatergory.Contains(subCategory));
            if (subCategoryCheck.Succeeded && subCategoryCheck.Result != null)
            {
                return AppResult<SubCategoryDTO>.CreateFailed(new ApplicationException("Can't create already existed sub category"), "Can't create already existed sub category");
            }
            var addSubCategory = new Entities.SubCategory
            {
                CatergoryId = categoryId,
                SubCatergory = subCategory
            };
            var createdSubCategory = await dataStore.SubCategory.Add(addSubCategory);
            if (!createdSubCategory.Succeeded || createdSubCategory.Result == null)
            {
                return AppResult<SubCategoryDTO>.CreateFailed(createdSubCategory.Error.Exception, createdSubCategory.Message);
            }
            return AppResult<SubCategoryDTO>.CreateSucceeded(new SubCategoryDTO
            {
               CategoryId = categoryId,
               SubCategory = subCategory
            }, "Successfully created experience subcategory");
        }
		catch (Exception ex)
		{
            return AppResult<SubCategoryDTO>.CreateFailed(ex, "An error occured when creating experience category");
        }
    }
    public async Task<AppResult<IEnumerable<SubCategoryDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.SubCategory.FindAsync(i => true, count, skip);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<SubCategoryDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var category = result.Result.Select(c =>
            {
                return new SubCategoryDTO
                {
                    Id= c.Id,
                    CategoryId = c.CatergoryId,
                    SubCategory = c.SubCatergory,
                };
            });

            return AppResult<IEnumerable<SubCategoryDTO>>.CreateSucceeded(category, "Successfully get sub-category");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<SubCategoryDTO>>.CreateFailed(ex, "An error occured in getting sub-category");
        }
    }
    public async Task<AppResult<IEnumerable<SubCategoryDTO>>> GetAllAsync()
    {
        try
        {
            var result = await dataStore.SubCategory.GetAllAsync();
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<SubCategoryDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
            var subCategories = result.Result.Select(a =>
            {
                return new SubCategoryDTO
                {
                    Id = a.Id,
                    CategoryId = a.CatergoryId,
                    SubCategory = a.SubCatergory
                };
            });
            return AppResult<IEnumerable<SubCategoryDTO>>.CreateSucceeded(subCategories, "Successfully get sub category");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<SubCategoryDTO>>.CreateFailed(ex, "An error occured in getting sub categories");
        }
    }
    public async Task<AppResult<SubCategoryDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.SubCategory.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<SubCategoryDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var subCategoryDTO = new SubCategoryDTO
            {
               Id = result.Result.Id,
               CategoryId = result.Result.CatergoryId,
               SubCategory = result.Result.SubCatergory
            };

            return AppResult<SubCategoryDTO>.CreateSucceeded(subCategoryDTO, "Successfully getting subcategory by id");
        }
        catch (Exception ex)
        {
            return AppResult<SubCategoryDTO>.CreateFailed(ex, "An error occured when getting subexperience category by id");
        }
    }
    public async Task<AppResult<SubCategoryDTO>> UpdateSubCategoryAsync(int id, int? categoryId, string? subCategory)
    {
        try
        {
            //check if experience category exist
            var subcategoryRes = await dataStore.SubCategory.GetByIdAsync(id);
            if (!subcategoryRes.Succeeded || subcategoryRes.Result == null)
            {
                return AppResult<SubCategoryDTO>.CreateFailed(new ApplicationException("Can't find subcategory to update"), "Can't find subcategory to update");
            }

            var subcategoryExeperience = subcategoryRes.Result;
            subcategoryExeperience.CatergoryId = categoryId ?? subcategoryExeperience.CatergoryId; 
            subcategoryExeperience.SubCatergory = subCategory ?? subcategoryExeperience.SubCatergory;

            var updatedsubCategory = await dataStore.SubCategory.Update(subcategoryExeperience);
            if (!updatedsubCategory.Succeeded)
            {
                return AppResult<SubCategoryDTO>.CreateFailed(updatedsubCategory.Error.Exception, updatedsubCategory.Message);
            }
            return AppResult<SubCategoryDTO>.CreateSucceeded(new SubCategoryDTO
            {
                
            }, "Successfully updated experience category");

        }
        catch (Exception ex)
        {
            return AppResult<SubCategoryDTO>.CreateFailed(ex, "An error occured when updating subcategory");
        }
    }
}

