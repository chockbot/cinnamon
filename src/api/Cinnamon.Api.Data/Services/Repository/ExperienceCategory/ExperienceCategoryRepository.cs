using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.ExperienceCategory.DTO;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.ExperienceCategory;
public class ExperienceCategoryRepository: IExperienceCategoryRepository
{
    private readonly IDataStore dataStore;

	public ExperienceCategoryRepository(IDataStore dataStore)
	{
		this.dataStore = dataStore;	
	}
    public async Task<AppResult<ExperienceCategoryDTO>> CreateExperienceCategoryAsync(string category, string iconPath)
    {
        try
        {
            // check if email already existed
            var categoryCheck = await dataStore.ExperienceCategory.FindFirstAsync(w => w.Category.Contains(category));
            if (categoryCheck.Succeeded && categoryCheck.Result != null)
            {
                return AppResult<ExperienceCategoryDTO>.CreateFailed(new ApplicationException("Can't create already existed experience category"), "Can't create already existed experience category");
            }
            var experienceCategory = new Entities.ExperienceCategory
            {
                Category = category,
                IconPath = iconPath
            };
            var createdCategory = await dataStore.ExperienceCategory.Add(experienceCategory);
            if (!createdCategory.Succeeded || createdCategory.Result == null)
            {
                return AppResult<ExperienceCategoryDTO>.CreateFailed(createdCategory.Error.Exception, createdCategory.Message);
            }
            return AppResult<ExperienceCategoryDTO>.CreateSucceeded(new ExperienceCategoryDTO
            {
                Category = category,
                IconPath = iconPath
            }, "Successfully created waitlist");
        }
        catch (Exception ex)
        {

            return AppResult<ExperienceCategoryDTO>.CreateFailed(ex, "An error occured when creating experience category");
        }
    }
    public async Task<AppResult<IEnumerable<ExperienceCategoryDTO>>> GetAllAsync(int? count, int? skip)
    {
        try
        {
            var result = await dataStore.ExperienceCategory.FindAsync(i => true, count, skip);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ExperienceCategoryDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var category = result.Result.Select(c =>
            {
                return new ExperienceCategoryDTO
                {
                   Id = c.Id,
                   Category = c.Category,
                   IconPath = c.IconPath
                };
            });

            return AppResult<IEnumerable<ExperienceCategoryDTO>>.CreateSucceeded(category, "Successfully get category");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ExperienceCategoryDTO>>.CreateFailed(ex, "An error occured in getting category");
        }
    }
    public async Task<AppResult<IEnumerable<ExperienceCategoryDTO>>> GetAllAsync()
	{
		try
		{
			var result = await dataStore.ExperienceCategory.GetAllAsync();
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ExperienceCategoryDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }
			var experienceCategories = result.Result.Select(a =>
			{
				return new ExperienceCategoryDTO
				{
					Id		 = a.Id,
					Category = a.Category,
					IconPath = a.IconPath
				};
			});
            return AppResult< IEnumerable<ExperienceCategoryDTO>>.CreateSucceeded(experienceCategories, "Successfully get experience category");
        }
		catch (Exception ex)
		{
            return AppResult<IEnumerable<ExperienceCategoryDTO>>.CreateFailed(ex, "An error occured in getting experience categories");
        }
	}
	public async Task<AppResult<ExperienceCategoryDTO>> GetByIdAsync(int id)
	{
        try
        {
            var result = await dataStore.ExperienceCategory.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<ExperienceCategoryDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var experienceCategoryDTO = new ExperienceCategoryDTO
            {
                Id = result.Result.Id,
                Category = result.Result.Category,
                IconPath = result.Result.IconPath,
            };

            return AppResult<ExperienceCategoryDTO>.CreateSucceeded(experienceCategoryDTO, "Successfully getting experience category by id");
        }
        catch (Exception ex)
        {
            return AppResult<ExperienceCategoryDTO>.CreateFailed(ex, "An error occured when getting experience category by id");
        }
    }
    public async Task<AppResult<ExperienceCategoryDTO>> UpdateExperienceCategoryAsync(int id, string? category, string? iconPath)
    {
        try
        {
            //check if experience category exist
            var categoryRes = await dataStore.ExperienceCategory.GetByIdAsync(id);
            if (!categoryRes.Succeeded || categoryRes.Result == null)
            {
                return AppResult<ExperienceCategoryDTO>.CreateFailed(new ApplicationException("Can't find experience category to update"), "Can't find experience category to update");
            }

            var categoryExeperience = categoryRes.Result;
            categoryExeperience.Category = category ?? categoryExeperience.Category;
            categoryExeperience.IconPath = iconPath ?? categoryExeperience.IconPath;

            var updatedcategoryExeperience = await dataStore.ExperienceCategory.Update(categoryExeperience);
            if (!updatedcategoryExeperience.Succeeded)
            {
                return AppResult<ExperienceCategoryDTO>.CreateFailed(updatedcategoryExeperience.Error.Exception, updatedcategoryExeperience.Message);
            }
            return AppResult<ExperienceCategoryDTO>.CreateSucceeded(new ExperienceCategoryDTO
            {
                Id = categoryExeperience.Id,
                Category = categoryExeperience.Category,
            }, "Successfully updated experience category");

        }
        catch (Exception ex)
        {
            return AppResult<ExperienceCategoryDTO>.CreateFailed(ex, "An error occured when updating experience category");
        }
    }
}

