using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ExperienceCreationType;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.ExperienceCreationType
{
    public class ExperienceCreationTypeRepository : IExperienceCreationTypeRepository
    {
        private readonly IDataStore dataStore;

        public ExperienceCreationTypeRepository(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }
        public async Task<AppResult<IEnumerable<ExperienceCreationTypeDTO>>> GetExperienceCreationTypes()
        {
            try
            {
                var result = await dataStore.ExperienceCreationType.FindAsync(e => e.IsActive);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<IEnumerable<ExperienceCreationTypeDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var experienceCreationTypes = result.Result.Select(r => new ExperienceCreationTypeDTO
                {
                    Name = r.Name,
                    Description = r.Description,
                    ImagePath = r.ImagePath
                });

                return AppResult<IEnumerable<ExperienceCreationTypeDTO>>.CreateSucceeded(experienceCreationTypes, "Successfully retrieved experience creation types");

            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ExperienceCreationTypeDTO>>.CreateFailed(ex, "An error occured when retrieving experience creation types");
            }
        }
    }
}
