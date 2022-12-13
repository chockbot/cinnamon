using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Description;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.ActivityDescription
{
    public class DescriptionRepository : IDescriptionRepository
    {
        private readonly IDataStore _dataStore;
        public DescriptionRepository(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<AppResult<DescriptionDTO>> CreateDescription(int ActivityId, string Description, string SpecificsYouWillProvide, string CustomerBringWithThem, string? AdditionalRequirements, string ActivityLevel, string SkillLevel, int MinimumAge, bool CanAdultsJoin)
        {
            try
            {
                var checkActivity = await _dataStore.Activity.GetByIdAsync(ActivityId);
                if(!checkActivity.Succeeded)
                {
                    return AppResult<DescriptionDTO>.CreateFailed(checkActivity.Error.Exception, checkActivity.Message);
                }

                var result = await _dataStore.ActivityDescription.Add(new Data.Repository.Entities.ActivityDescription()
                {
                    ActivityId = ActivityId,
                    Description = Description,
                    SpecificsYouWillProvide = SpecificsYouWillProvide,
                    CustomerBringWithThem = CustomerBringWithThem,
                    AdditionalRequirements = AdditionalRequirements,
                    ActivityLevel = ActivityLevel,
                    MinimumAge = MinimumAge,
                    CanAdultsJoin = CanAdultsJoin
                });

                return AppResult<DescriptionDTO>.CreateSucceeded(new DescriptionDTO()
                {
                    Id = result.Result.Id,
                    ActivityId = result.Result.ActivityId,
                    Description = result.Result.Description,
                    SpecificsYouWillProvide = result.Result.SpecificsYouWillProvide,
                    CustomerBringWithThem = result.Result.CustomerBringWithThem,
                    AdditionalRequirements = result.Result.AdditionalRequirements,
                    ActivityLevel = result.Result.ActivityLevel,
                    MinimumAge = result.Result.MinimumAge,
                    SkillLevel = result.Result.SkillLevel,
                    CanAdultsJoin = result.Result.CanAdultsJoin
                },
                checkActivity.Message);
            }
            catch(Exception ex)
            {
                return AppResult<DescriptionDTO>.CreateFailed(ex.InnerException, ex.Message);
            }
        }

        public async Task<AppResult<IEnumerable<DescriptionDTO>>> GetAllAsync()
        {
            try
            {
                var result = await _dataStore.ActivityDescription.GetAllAsync();
                if (!result.Succeeded)
                {
                    return AppResult<IEnumerable<DescriptionDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var Descriptions = result.Result.Select(x =>
                {
                    return new DescriptionDTO
                    {
                        Id = x.Id,
                        ActivityId = x.ActivityId,
                        Description = x.Description,
                        SpecificsYouWillProvide = x.SpecificsYouWillProvide,
                        CustomerBringWithThem = x.CustomerBringWithThem,
                        AdditionalRequirements = x.AdditionalRequirements,
                        CanAdultsJoin = x.CanAdultsJoin,
                        ActivityLevel = x.ActivityLevel,
                        MinimumAge = x.MinimumAge,
                        SkillLevel = x.SkillLevel
                    };
                });

                return AppResult<IEnumerable<DescriptionDTO>>.CreateSucceeded(Descriptions, "Successfully get descriptions");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<DescriptionDTO>>.CreateFailed(ex.InnerException, ex.Message);
            }
        }

        public async Task<AppResult<DescriptionDTO>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _dataStore.ActivityDescription.GetByIdAsync(id);
                if(!result.Succeeded)
                {
                    return AppResult<DescriptionDTO>.CreateFailed(result.Error.Exception, result.Message);
                }

                return AppResult<DescriptionDTO>.CreateSucceeded(new DescriptionDTO()
                {
                    Id = result.Result.Id,
                    ActivityId = result.Result.ActivityId,
                    Description = result.Result.Description,
                    SpecificsYouWillProvide = result.Result.SpecificsYouWillProvide,
                    CustomerBringWithThem = result.Result.CustomerBringWithThem,
                    AdditionalRequirements = result.Result.AdditionalRequirements,
                    ActivityLevel = result.Result.ActivityLevel,
                    MinimumAge = result.Result.MinimumAge,
                    SkillLevel = result.Result.SkillLevel,
                    CanAdultsJoin = result.Result.CanAdultsJoin
                },
                result.Message);
            }
            catch (Exception ex)
            {
                return AppResult<DescriptionDTO>.CreateFailed(ex.InnerException, ex.Message);
            }
        }

        public async Task<AppResult<DescriptionDTO>> UpdateDescription(int DescriptionId, string Description, string SpecificsYouWillProvide, string CustomerBringWithThem, string? AdditionalRequirements, string ActivityLevel, string SkillLevel, int MinimumAge, bool CanAdultsJoin)
        {
            try
            {
                var checkDescription = await _dataStore.ActivityDescription.GetByIdAsync(DescriptionId);
                if(!checkDescription.Succeeded)
                {
                    return AppResult<DescriptionDTO>.CreateFailed(checkDescription.Error.Exception, checkDescription.Message);
                }

                var result = await _dataStore.ActivityDescription.Update(new Data.Repository.Entities.ActivityDescription()
                {
                    Id = DescriptionId,
                    ActivityId = checkDescription.Result.ActivityId,
                    Description = Description,
                    SpecificsYouWillProvide = SpecificsYouWillProvide,
                    CustomerBringWithThem = CustomerBringWithThem,
                    AdditionalRequirements = AdditionalRequirements,
                    ActivityLevel = ActivityLevel,
                    MinimumAge = MinimumAge,
                    SkillLevel = SkillLevel,
                    CanAdultsJoin = CanAdultsJoin
                });
                
                if(!result.Succeeded)
                {
                    return AppResult<DescriptionDTO>.CreateFailed(result.Error.Exception, "Error updating the description");
                }

                return AppResult<DescriptionDTO>.CreateSucceeded(new DescriptionDTO()
                {
                    Id = result.Result.Id,
                    ActivityId = result.Result.ActivityId,
                    Description = result.Result.Description,
                    SpecificsYouWillProvide = result.Result.SpecificsYouWillProvide,
                    CustomerBringWithThem = result.Result.CustomerBringWithThem,
                    AdditionalRequirements = result.Result.AdditionalRequirements,
                    ActivityLevel = result.Result.ActivityLevel,
                    MinimumAge = result.Result.MinimumAge,
                    SkillLevel = result.Result.SkillLevel,
                    CanAdultsJoin = result.Result.CanAdultsJoin
                }, 
                "Successfully updated");
            }
            catch (Exception ex)
            {
                return AppResult<DescriptionDTO>.CreateFailed(ex.InnerException, ex.Message);
            }
        }
    }
}
