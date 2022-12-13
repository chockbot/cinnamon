using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.ExperienceType.DTO;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.ExperienceType
{
    public class ExperienceTypeRepository : IExperienceTypeRepository
    {
        private readonly IDataStore _dataStore;
        public ExperienceTypeRepository(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<AppResult<ExperienceTypeDTO>> CreateExperienceType(string name)
        {
            try
            {
                var result =await  _dataStore.ExperienceType.Add(new Data.Repository.Entities.ExperienceType()
                {
                    Name = name
                });

                if(!result.Succeeded)
                {
                    return AppResult<ExperienceTypeDTO>.CreateFailed(result.Error.Exception, result.Message);
                }

                return AppResult<ExperienceTypeDTO>.CreateSucceeded(new ExperienceTypeDTO()
                {
                    Id = result.Result.Id,
                    Name = result.Result.Name
                }, result.Message);
            }
            catch (Exception ex)
            {
                return AppResult<ExperienceTypeDTO>.CreateFailed(ex.InnerException, ex.Message);
            }
        }

        public async Task<AppResult<IEnumerable<ExperienceTypeDTO>>> GetAllAsync()
        {
            try
            {
                var result = await _dataStore.ExperienceType.GetAllAsync();
                if(!result.Succeeded)
                {
                    return AppResult<IEnumerable<ExperienceTypeDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var ExperienceTypes = result.Result.Select(x =>
                {
                    return new ExperienceTypeDTO
                    {
                        Id = x.Id,
                        Name = x.Name
                    };
                });
                return AppResult<IEnumerable<ExperienceTypeDTO>>.CreateSucceeded(ExperienceTypes, result.Message);
            }
            catch(Exception ex)
            {
                return AppResult<IEnumerable<ExperienceTypeDTO>>.CreateFailed(ex.InnerException, ex.Message);
            }
        }

        public async Task<AppResult<ExperienceTypeDTO>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _dataStore.ExperienceType.GetByIdAsync(id);
                if(!result.Succeeded)
                {
                    return AppResult<ExperienceTypeDTO>.CreateFailed(result.Error.Exception, result.Message);
                }

                return AppResult<ExperienceTypeDTO>.CreateSucceeded(new ExperienceTypeDTO()
                {
                    Id = result.Result.Id,
                    Name = result.Result.Name
                }, 
                result.Message);
            }
            catch (Exception ex)
            {
                return AppResult<ExperienceTypeDTO>.CreateFailed(ex.InnerException, ex.Message);
            }
        }

        public async Task<AppResult<ExperienceTypeDTO>> UpdateExperienceType(int Id, string name)
        {
            try
            {
                var checkType = await _dataStore.ExperienceType.GetByIdAsync(Id);
                if(!checkType.Succeeded)
                {
                    return AppResult<ExperienceTypeDTO>.CreateFailed(checkType.Error.Exception, checkType.Message);
                }

                var result = await _dataStore.ExperienceType.Update(new Data.Repository.Entities.ExperienceType()
                {
                    Id = Id,
                    Name = name
                });

                if(!result.Succeeded)
                {
                    return AppResult<ExperienceTypeDTO>.CreateFailed(result.Error.Exception, result.Message);
                }

                return AppResult<ExperienceTypeDTO>.CreateSucceeded(new ExperienceTypeDTO()
                {
                    Id = result.Result.Id,
                    Name = result.Result.Name
                }, 
                result.Message);
            }
            catch (Exception ex)
            {
                return AppResult<ExperienceTypeDTO>.CreateFailed(ex.InnerException, ex.Message);
            }
        }
    }
}
