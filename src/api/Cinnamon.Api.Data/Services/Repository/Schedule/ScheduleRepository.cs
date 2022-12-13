using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Schedule.DTO;
using Cinnamon.Framework.Common;
using System.Collections.Generic;

namespace Cinnamon.Api.Data.Services.Repository.Schedule
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IDataStore _dataStore;
        public ScheduleRepository(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<AppResult<ScheduleDTO>> CreateSchedule(int ActivityId, string Name, string datetime, 
                            decimal Price, string UnitPrice, int PerUnit1, string PriceUnit1, 
                            int PerUnit2, string PriceUnit2)
        {
            try
            {
                var checkActivity = await _dataStore.Activity.GetByIdAsync(ActivityId);
                if(!checkActivity.Succeeded)
                {
                    return AppResult<ScheduleDTO>.CreateFailed(checkActivity.Error.Exception, checkActivity.Message);
                }

                var result = await _dataStore.ActivitySchedule.Add(new Data.Repository.Entities.ActivitySchedule()
                {
                    ActivityId = ActivityId,
                    Name = Name,
                    DateTime = datetime,
                    Price = Price,
                    UnitPrice = UnitPrice,
                    PerUnit1 = PerUnit1,
                    PriceUnit1 = PriceUnit1,
                    PerUnit2 = PerUnit2,
                    PriceUnit2 = PriceUnit2
                });

                if(!result.Succeeded)
                {
                    return AppResult<ScheduleDTO>.CreateFailed(result.Error.Exception, result.Message);
                }

                return AppResult<ScheduleDTO>.CreateSucceeded(new ScheduleDTO()
                {
                    Id = result.Result.Id,
                    ActivityId = result.Result.ActivityId,
                    Name = result.Result.Name,
                    DateTime = result.Result.DateTime,
                    Price = result.Result.Price,
                    UnitPrice = result.Result.UnitPrice,
                    PerUnit1 = result.Result.PerUnit1,
                    PriceUnit1 = result.Result.PriceUnit1,
                    PerUnit2 = result.Result.PerUnit2,
                    PriceUnit2 = result.Result.PriceUnit2
                },
                result.Message);
            }
            catch (Exception ex)
            {
                return AppResult<ScheduleDTO>.CreateFailed(ex.InnerException, ex.Message);
            }
        }

        public async Task<AppResult<IEnumerable<ScheduleDTO>>> GetAllAsync()
        {
            try
            {
                var result = await _dataStore.ActivitySchedule.GetAllAsync();
                if (!result.Succeeded)
                {
                    return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(result.Error.Exception, result.Message);
                }

                var Schedules = result.Result.Select(x =>
                {
                    return new ScheduleDTO
                    {
                        Id = x.Id,
                        ActivityId = x.ActivityId,
                        Name = x.Name,
                        Price = x.Price,
                        DateTime = x.DateTime,
                        UnitPrice = x.UnitPrice,
                        PerUnit1 = x.PerUnit1,
                        PriceUnit1 = x.PriceUnit1,
                        PerUnit2 = x.PerUnit2,
                        PriceUnit2 = x.PriceUnit2
                    };
                });

                return AppResult<IEnumerable<ScheduleDTO>>.CreateSucceeded(Schedules, "Success");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(ex.InnerException, ex.Message);
            }
        }

        public async Task<AppResult<ScheduleDTO>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _dataStore.ActivitySchedule.GetByIdAsync(id);
                if(!result.Succeeded)
                {
                    return AppResult<ScheduleDTO>.CreateFailed(result.Error.Exception, result.Message);
                }

                return AppResult<ScheduleDTO>.CreateSucceeded(new ScheduleDTO()
                {
                    Id = result.Result.Id,
                    ActivityId = result.Result.ActivityId,
                    Name = result.Result.Name,
                    DateTime = result.Result.DateTime,
                    Price = result.Result.Price,
                    UnitPrice = result.Result.UnitPrice,
                    PerUnit1 = result.Result.PerUnit1,
                    PriceUnit1 = result.Result.PriceUnit1,
                    PerUnit2 = result.Result.PerUnit2,
                    PriceUnit2 = result.Result.PriceUnit2
                },
                result.Message); 
            }
            catch (Exception ex)
            {
                return AppResult<ScheduleDTO>.CreateFailed(ex.InnerException, ex.Message);
            }
        }

        public async Task<AppResult<ScheduleDTO>> UpdateSchedule(int ScheduleId, string Name, string datetime, 
                                    decimal Price, string UnitPrice, int PerUnit1, string PriceUnit1, 
                                    int PerUnit2, string PriceUnit2)
        {
            try
            {
                var checkSchedule = await _dataStore.ActivitySchedule.GetByIdAsync(ScheduleId);
                if(!checkSchedule.Succeeded)
                {
                    return AppResult<ScheduleDTO>.CreateFailed(checkSchedule.Error.Exception, checkSchedule.Message);
                }

                var result = await _dataStore.ActivitySchedule.Update(new Data.Repository.Entities.ActivitySchedule()
                {
                    Id = ScheduleId,
                    ActivityId = checkSchedule.Result.ActivityId,
                    Name = Name,
                    DateTime = datetime,
                    Price = Price,
                    UnitPrice = UnitPrice,
                    PerUnit1 = PerUnit1,
                    PriceUnit1 = PriceUnit1,
                    PerUnit2 = PerUnit2,
                    PriceUnit2 = PriceUnit2
                });

                if (!result.Succeeded)
                {
                    return AppResult<ScheduleDTO>.CreateFailed(result.Error.Exception, result.Message);
                }

                return AppResult<ScheduleDTO>.CreateSucceeded(new ScheduleDTO()
                {
                    Id = result.Result.Id,
                    ActivityId = result.Result.ActivityId,
                    Name = result.Result.Name,
                    DateTime = result.Result.DateTime,
                    Price = result.Result.Price,
                    UnitPrice = result.Result.UnitPrice,
                    PerUnit1 = result.Result.PerUnit1,
                    PriceUnit1 = result.Result.PriceUnit1,
                    PerUnit2 = result.Result.PerUnit2,
                    PriceUnit2 = result.Result.PriceUnit2
                },
                result.Message);
            }
            catch (Exception ex)
            {
                return AppResult<ScheduleDTO>.CreateFailed(ex.InnerException, ex.Message);
            }
        }
    }
}
