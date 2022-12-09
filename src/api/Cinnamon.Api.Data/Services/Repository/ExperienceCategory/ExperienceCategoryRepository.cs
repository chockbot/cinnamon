using Cinnamon.Api.Data.Services.Repository.ExperienceCategory.DTO;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Services.Repository.Activity.DTO;

namespace Cinnamon.Api.Data.Services.Repository.ExperienceCategory;
public class ExperienceCategoryRepository: IExperienceCategoryRepository
{
    private readonly IDataStore dataStore;

	public ExperienceCategoryRepository(IDataStore dataStore)
	{
		this.dataStore = dataStore;	
	}
    //Select
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
    //Insert/Update
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
    //Save
    //public async Task<AppResult<ExperienceCategoryDTO>> SaveDataAsync()
    //{
    //    try
    //    {

    //    }
    //    catch (Exception ex)
    //    {

    //        throw;
    //    }
    //}
}

