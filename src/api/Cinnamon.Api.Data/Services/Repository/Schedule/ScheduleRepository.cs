using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Schedule;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;
using Entities = Cinnamon.Api.Data.Repository.Entities;

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
                            int PerUnit2, string PriceUnit2, int order, bool IsActiveSchedule)
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
                    PriceUnit2 = PriceUnit2,
                    Order = order,
                    IsActiveSchedule = IsActiveSchedule
                    
                });

                if(!result.Succeeded || result.Result == null)
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
                    PriceUnit2 = result.Result.PriceUnit2,
                    Order = result.Result.Order,
                    IsActiveSchedule = result.Result.IsActiveSchedule
                },
                result.Message);
            }
            catch (Exception ex)
            {
                return AppResult<ScheduleDTO>.CreateFailed(ex.InnerException, ex.Message);
            }
        }

        public async Task<AppResult<IEnumerable<ScheduleDTO>>> CreateSchedules(int activityId, IEnumerable<ScheduleDTO> schedules)
        {
            try
            {
                // check activity id if existed
                var activity = await _dataStore.Activity.GetByIdAsync(activityId);
                if(!activity.Succeeded || activity.Result == null)
                {
                    return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(
                        new ApplicationException("Can't find provided activity id"), "Can't find provided activity id");
                }

                var entities = schedules.Select(s => {
                    return new Entities.ActivitySchedule {
                        ActivityId = activityId,
                        DateTime = s.DateTime,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        PerUnit2 = s.PerUnit2,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice,
                        Order = s.Order,
                        IsActiveSchedule = s.IsActiveSchedule
                    };
                });

                var result = await _dataStore.ActivitySchedule.AddRange(entities);
                if(!result.Succeeded || result.Result == null)
                {
                    return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(new ApplicationException(result.Message), "An error occured when creating multiple schedules");
                }

                var dtos = result.Result.Select(s => {
                    return new ScheduleDTO {
                        ActivityId = s.ActivityId,
                        DateTime = s.DateTime,
                        Id = s.Id,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        PerUnit2 = s.PerUnit2,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice,
                        Order = s.Order,
                        IsActiveSchedule = s.IsActiveSchedule
                    };
                });

                return AppResult<IEnumerable<ScheduleDTO>>.CreateSucceeded(dtos, "Successfully create multiple schedules");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(ex, "An error occured when creating multiple schedules");
            }
        }

        public async Task<AppResult<bool>> DeleteManySchedules(IEnumerable<int> schedulesIds)
        {
            try
            {
                if(schedulesIds == null || schedulesIds.Count() == 0)
                {
                    return AppResult<bool>.CreateFailed(new ApplicationException("No schedules to delete"), "No schedules to delete");
                }
                schedulesIds = schedulesIds.Where(i => i > 0);
                if(schedulesIds.Count() == 0)
                {
                    return AppResult<bool>.CreateFailed(new ApplicationException("No schedules to delete"), "No schedules to delete");
                }

                var result = await _dataStore.ActivitySchedule.RemoveRange(schedulesIds.Select(i => {
                    return new Entities.ActivitySchedule {
                        Id = i
                    };
                }));
                
                if(!result.Succeeded || result.Result == null)
                {
                    return AppResult<bool>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                return AppResult<bool>.CreateSucceeded(true, "Successfully deleted schedules");
            }
            catch (Exception ex)
            {
                return AppResult<bool>.CreateFailed(ex, "An error occured in deleting many schedules");
            }
        }

        public async Task<AppResult<IEnumerable<ScheduleDTO>>> GetAllAsync()
        {
            try
            {
                var result = await _dataStore.ActivitySchedule.GetAllAsync();
                if (!result.Succeeded || result.Result == null)
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
                        PriceUnit2 = x.PriceUnit2,
                        IsActiveSchedule = x.IsActiveSchedule
                    };
                });

                return AppResult<IEnumerable<ScheduleDTO>>.CreateSucceeded(Schedules, "Success");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(ex.InnerException, ex.Message);
            }
        }

        public async Task<AppResult<ScheduleDTO>> GetByIdAsync(int id, bool? includeActivity = null)
        {
            try
            {
                var includes = new List<Expression<Func<Entities.ActivitySchedule, object>>>();
                if(includeActivity.HasValue && includeActivity.Value) includes.Add(s => s.Activity);

                var result = await _dataStore.ActivitySchedule.FindFirstAsync(s => s.Id == id, includes);
                if(!result.Succeeded || result.Result == null)
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
                    PriceUnit2 = result.Result.PriceUnit2,
                    Order = result.Result.Order,
                    IsActiveSchedule = result.Result.IsActiveSchedule  
                },
                result.Message); 
            }
            catch (Exception ex)
            {
                return AppResult<ScheduleDTO>.CreateFailed(ex, ex.Message);
            }
        }

        public async Task<AppResult<IEnumerable<ScheduleDTO>>> UpdateManySchedules(IEnumerable<ScheduleDTO> args)
        {
            try
            {
                if(args.Count() <= 0)
                {
                    return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(new ApplicationException("No schedules to update"), "No schedules to update");
                }

                var scheduleToUpdate = args.Select(s => {
                    return new Entities.ActivitySchedule {
                        DateTime = s.DateTime,
                        Id = s.Id,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        PerUnit2 = s.PerUnit2,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice,
                        ActivityId = s.ActivityId,
                        Order = s.Order,
                        IsActiveSchedule = s.IsActiveSchedule
                    };
                });

                var updated = await _dataStore.ActivitySchedule.UpdateRange(scheduleToUpdate);
                if(!updated.Succeeded || updated.Result == null)
                {
                    return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(new ApplicationException(updated.Message), updated.Message);
                }

                return AppResult<IEnumerable<ScheduleDTO>>.CreateSucceeded(
                    updated.Result.Select(s => {
                        return new ScheduleDTO {
                            ActivityId = s.ActivityId,
                            DateTime = s.DateTime,
                            Id = s.Id,
                            Name = s.Name,
                            PerUnit1 = s.PerUnit1,
                            PerUnit2 = s.PerUnit2,
                            Price = s.Price,
                            PriceUnit1 = s.PriceUnit1,
                            PriceUnit2 = s.PriceUnit2,
                            UnitPrice = s.UnitPrice,
                            Order = s.Order,
                            IsActiveSchedule = s.IsActiveSchedule
                        };
                    }), "Successfully update many schedules"
                );
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(ex, "An error occured in updating many schedules");
            }
        }

        public async Task<AppResult<ScheduleDTO>> UpdateSchedule(int ScheduleId, string Name, string datetime, 
                                    decimal Price, string UnitPrice, int PerUnit1, string PriceUnit1, 
                                    int PerUnit2, string PriceUnit2, int order, bool IsActiveSchedule)
        {
            try
            {
                var checkSchedule = await _dataStore.ActivitySchedule.GetByIdAsync(ScheduleId);
                if(!checkSchedule.Succeeded || checkSchedule.Result == null)
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
                    PriceUnit2 = PriceUnit2,
                    Order = order,
                    IsActiveSchedule = IsActiveSchedule
                });

                if (!result.Succeeded || result.Result == null)
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
                    PriceUnit2 = result.Result.PriceUnit2,
                    Order = result.Result.Order,
                    IsActiveSchedule = result.Result.IsActiveSchedule
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
