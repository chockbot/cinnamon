using Cinnamon.Api.Data.Extensions;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Schedule;
using Cinnamon.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
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
                            int PerUnit2, string PriceUnit2, int order, bool IsActiveSchedule,bool IsSetSession, string SessionName, int HasExpiration, DateTime? startDate)
        {
            try
            {
                var checkActivity = await _dataStore.Activity.GetByIdAsync(ActivityId);
                if(!checkActivity.Succeeded)
                {
                    return AppResult<ScheduleDTO>.CreateFailed(checkActivity.Error.Exception, checkActivity.Message);
                }

                startDate = startDate.SetKindUtc();

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
                    IsActiveSchedule = IsActiveSchedule,
                    IsSetSession = IsSetSession,
                    SessionName = SessionName,
                    HasExpiration = HasExpiration,
                    StartDate = startDate,  
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
                    IsActiveSchedule = result.Result.IsActiveSchedule,
                    IsSetSession= result.Result. IsSetSession,
                    SessionName = result.Result.SessionName,
                    HasExpiration = result.Result.HasExpiration,
                    StartDate = result.Result.StartDate,
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
                List<Entities.ActivitySchedule> dtoList = new List<Entities.ActivitySchedule>();
                
                if (!activity.Succeeded || activity.Result == null)
                {
                    return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(
                        new ApplicationException("Can't find provided activity id"), "Can't find provided activity id");
                }

                foreach (var schedule in schedules)
                {
                    var entity = new Entities.ActivitySchedule
                    {
                        ActivityId       = activityId,
                        DateTime         = schedule.DateTime,
                        Name             = schedule.Name,
                        PerUnit1         = schedule.PerUnit1,
                        PerUnit2         = schedule.PerUnit2,
                        Price            = schedule.Price,
                        PriceUnit1       = schedule.PriceUnit1,
                        PriceUnit2       = schedule.PriceUnit2,
                        UnitPrice        = schedule.UnitPrice,
                        Order            = schedule.Order,
                        IsActiveSchedule = schedule.IsActiveSchedule,
                        IsSetSession     = schedule.IsSetSession,
                        SessionName      = schedule.SessionName ?? string.Empty,
                        HasExpiration    = schedule.HasExpiration,
                        StartDate        = schedule.StartDate.SetKindUtc(),
                        ScheduleType     = (int)schedule.ScheduleType,
                        PriceType        = (int)schedule.PriceType,
                    };

                    var result = await _dataStore.ActivitySchedule.Add(entity);

                    if (!result.Succeeded || result.Result == null)
                    {
                        return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(new ApplicationException(result.Message), "An error occured when creating multiple schedules");
                    }

                    dtoList.Add(result.Result);

                    if (schedule.ActivityScheduleTimes.Count() > 0)
                    {
                        var scheduleTimeEntity = schedule.ActivityScheduleTimes.Select(a =>
                        {
                            return new Entities.ActivityScheduleTime
                            {
                                ActivityScheduleId = result.Result.Id,
                                DayOfWeek = a.DayOfWeek,
                                StartTime = a.StartTime,
                                EndTime = a.EndTime,
                                IsEnabled = a.IsEnabled,
                            };
                        });

                        var scheduleTimeResult = await _dataStore.ActivityScheduleTime.AddRange(scheduleTimeEntity);

                        if (!scheduleTimeResult.Succeeded || scheduleTimeResult.Result == null)
                        {
                            return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(new ApplicationException(result.Message), "An error occured when creating multiple schedules");
                        }
                    }
                }
                
                var dtos = dtoList.Select(s => {
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
                        IsActiveSchedule = s.IsActiveSchedule,
                        IsSetSession = s.IsSetSession,  
                        SessionName = s.SessionName,    
                        HasExpiration = s.HasExpiration,   
                        StartDate = s.StartDate.SetKindUtc()
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

        public async Task<AppResult<ScheduleDTO>> GetActivityScheduleTimes(int activityScheduleId, int dayOfWeek, DateTime scheduleDate)
        {
            try
            {
                var result = new ScheduleDTO();

                var activityScheduleResult = await _dataStore.ActivityScheduleTime.FindAsync(a => a.ActivityScheduleId == activityScheduleId && a.DayOfWeek == dayOfWeek);

                if (!activityScheduleResult.Succeeded || activityScheduleResult.Result == null)
                {
                    return AppResult<ScheduleDTO>.CreateFailed(
                        new ApplicationException("Can't find activity schedule"), "Can't find activity schedule");
                }

                var activityScheduleTime = activityScheduleResult.Result;

                var ongoingActivityResult = await _dataStore.OngoingActivityScheduleTime.FindAsync(a => activityScheduleTime.Select(s => s.Id).Contains(a.ActivityScheduleTimeId) && a.ScheduleDate == scheduleDate.SetKindUtc() && !a.IsCompleted);

                if (!ongoingActivityResult.Succeeded || ongoingActivityResult.Result == null)
                {
                    return AppResult<ScheduleDTO>.CreateFailed(
                        new ApplicationException("Can't find activity schedule"), "Can't find activity schedule");
                }

                var ongoingActivitySchedule = ongoingActivityResult.Result;

                foreach (var scheduleTime in activityScheduleTime)
                {
                    result.ActivityScheduleTimes.Add(new ActivityScheduleTimeDTO
                    {
                        ActivityScheduleTimeId = scheduleTime.Id,
                        ActivityScheduleId = scheduleTime.ActivityScheduleId,
                        DayOfWeek = scheduleTime.DayOfWeek,
                        EndTime = scheduleTime.EndTime,
                        StartTime = scheduleTime.StartTime,
                        IsAvailable = ongoingActivitySchedule.Select(o => o.ActivityScheduleTimeId).Contains(scheduleTime.Id) ? false : true
                    });
                }

                return AppResult<ScheduleDTO>.CreateSucceeded(result, "Successfully retrieved activity schedules");
            }
            catch (Exception ex)
            {
                return AppResult<ScheduleDTO>.CreateFailed(ex, "An error occured in find activity schedule");
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
                        IsActiveSchedule = x.IsActiveSchedule,
                        IsSetSession = x.IsSetSession,
                        SessionName = x.SessionName,
                        HasExpiration = x.HasExpiration,
                        StartDate = x.StartDate,
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
                    IsActiveSchedule = result.Result.IsActiveSchedule,
                    IsSetSession = result.Result.IsSetSession,
                    SessionName = result.Result.SessionName,
                    HasExpiration = result.Result.HasExpiration,
                    StartDate = result.Result.StartDate,    
                },
                result.Message); 
            }
            catch (Exception ex)
            {
                return AppResult<ScheduleDTO>.CreateFailed(ex, ex.Message);
            }
        }

        public async Task<AppResult<bool>> CreateOngoingActivitySchedule(DateTime scheduleDate, int activityScheduleTimeId, int purchaseOrderId, bool isCompleted, int createdBy)
        {
            try
            {
                var scheduleTimeResult = await _dataStore.ActivityScheduleTime.FindFirstAsync(s => s.Id == activityScheduleTimeId);
                if (!scheduleTimeResult.Succeeded || scheduleTimeResult.Result == null)
                {
                    return AppResult<bool>.CreateFailed(scheduleTimeResult.Error.Exception, scheduleTimeResult.Message);
                }

                var result = await _dataStore.OngoingActivityScheduleTime.Add(new Entities.OngoingActivityScheduleTime
                {
                    ScheduleDate = scheduleDate.SetKindUtc(),
                    ActivityScheduleTimeId = activityScheduleTimeId,
                    PurchaseOrderId = purchaseOrderId,
                    IsCompleted = isCompleted,
                    CreatedBy = createdBy
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<bool>.CreateFailed(result.Error.Exception, result.Message);
                }

                return AppResult<bool>.CreateSucceeded(result.Succeeded, result.Message);
            }
            catch (Exception ex)
            {
                return AppResult<bool>.CreateFailed(ex, ex.Message);
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
                        Id = s.Id,
                        DateTime = s.DateTime,
                        Name = s.Name,
                        PerUnit1 = s.PerUnit1,
                        PerUnit2 = s.PerUnit2,
                        Price = s.Price,
                        PriceUnit1 = s.PriceUnit1,
                        PriceUnit2 = s.PriceUnit2,
                        UnitPrice = s.UnitPrice,
                        ActivityId = s.ActivityId,
                        Order = s.Order,
                        IsActiveSchedule = s.IsActiveSchedule,
                        IsSetSession = s.IsSetSession,
                        SessionName = s.SessionName,
                        HasExpiration = s.HasExpiration,
                        StartDate = s.StartDate.SetKindUtc(),
                        PriceType = (int)s.PriceType,
                        ScheduleType = (int)s.ScheduleType,
                    };
                });

                var updated = await _dataStore.ActivitySchedule.UpdateRange(scheduleToUpdate);

                foreach (var schedule in args)
                {
                    if (schedule.ActivityScheduleTimes.Any())
                    {
                        var scheduleId = schedule.Id;

                        var createdScheduleTimes = schedule.ActivityScheduleTimes.Where(a => a.ModelStatus == Framework.Enums.Enums.ModelStatus.Created)
                                                   .Select(a => new Entities.ActivityScheduleTime
                                                   {
                                                       ActivityScheduleId = scheduleId,
                                                       DayOfWeek = a.DayOfWeek,
                                                       StartTime = a.StartTime,
                                                       EndTime = a.EndTime,
                                                       IsEnabled = a.IsEnabled
                                                   });

                        var updatedScheduleTimes = schedule.ActivityScheduleTimes.Where(a => a.ModelStatus == Framework.Enums.Enums.ModelStatus.Updated)
                                                   .Select(a => new Entities.ActivityScheduleTime
                                                   {
                                                       Id = a.ActivityScheduleTimeId,
                                                       ActivityScheduleId = scheduleId,
                                                       DayOfWeek = a.DayOfWeek,
                                                       StartTime = a.StartTime,
                                                       EndTime = a.EndTime,
                                                       IsEnabled = a.IsEnabled
                                                   });

                        var deletedScheduleTimes = schedule.ActivityScheduleTimes.Where(a => a.ModelStatus == Framework.Enums.Enums.ModelStatus.Deleted)
                                                  .Select(a => new Entities.ActivityScheduleTime
                                                  {
                                                      Id = a.ActivityScheduleTimeId,
                                                      ActivityScheduleId = scheduleId,
                                                      DayOfWeek = a.DayOfWeek,
                                                      StartTime = a.StartTime,
                                                      EndTime = a.EndTime,
                                                      IsEnabled = a.IsEnabled
                                                  });

                        var scheduleTimeCreateResult = await _dataStore.ActivityScheduleTime.AddRange(createdScheduleTimes);
                        var scheduleTimeUpdateResult = await _dataStore.ActivityScheduleTime.UpdateRange(updatedScheduleTimes);
                        var scheduleTimeDeleteResult = await _dataStore.ActivityScheduleTime.RemoveRange(deletedScheduleTimes);

                        if (!scheduleTimeCreateResult.Succeeded || scheduleTimeCreateResult.Result == null)
                        {
                            return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(new ApplicationException(scheduleTimeCreateResult.Message), scheduleTimeCreateResult.Message);
                        }

                        if (!scheduleTimeUpdateResult.Succeeded || scheduleTimeUpdateResult.Result == null)
                        {
                            return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(new ApplicationException(scheduleTimeUpdateResult.Message), scheduleTimeUpdateResult.Message);
                        }

                        if (!scheduleTimeDeleteResult.Succeeded || scheduleTimeDeleteResult.Result == null)
                        {
                            return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(new ApplicationException(scheduleTimeDeleteResult.Message), scheduleTimeDeleteResult.Message);
                        }
                    }
                }
              
                if (!updated.Succeeded || updated.Result == null)
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
                            IsActiveSchedule = s.IsActiveSchedule,
                            IsSetSession= s.IsSetSession,
                            HasExpiration= s.HasExpiration,
                            SessionName = s.SessionName,
                            StartDate= s.StartDate.SetKindUtc(),
                        };
                    }), "Successfully update many schedules"
                );
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ScheduleDTO>>.CreateFailed(ex, "An error occured in updating many schedules");
            }
        }

        public async Task<AppResult<ScheduleDTO>> UpdateSchedule(int? ScheduleId, string? Name, string? datetime, decimal? Price, 
                                                                string? UnitPrice, int? PerUnit1, string? PriceUnit1, int? PerUnit2, 
                                                                string? PriceUnit2, int? order, bool? IsActiveSchedule, bool? IsSetSession, string? SessionName, int? HasExpiration, DateTime? startDate)
        {
            try
            {
                var checkSchedule = await _dataStore.ActivitySchedule.GetByIdAsync(ScheduleId.GetValueOrDefault());
                if(!checkSchedule.Succeeded || checkSchedule.Result == null)
                {
                    return AppResult<ScheduleDTO>.CreateFailed(checkSchedule.Error.Exception, checkSchedule.Message);
                }
                startDate = startDate.SetKindUtc();

                var schedule = checkSchedule.Result;

                schedule.Id               = ScheduleId.GetValueOrDefault();
                schedule.ActivityId       = schedule.ActivityId;
                schedule.Name             = Name ?? schedule.Name;
                schedule.DateTime         = datetime ?? schedule.DateTime;
                schedule.Price            = Price ?? schedule.Price;
                schedule.UnitPrice        = UnitPrice ?? schedule.UnitPrice;
                schedule.PerUnit1         = PerUnit1 ?? schedule.PerUnit1;
                schedule.PriceUnit1       = PriceUnit1 ?? schedule.PriceUnit1;
                schedule.PerUnit2         = PerUnit2 ?? schedule.PerUnit2;
                schedule.PriceUnit2       = PriceUnit2 ?? schedule.PriceUnit2;
                schedule.Order            = order ?? schedule.Order;
                schedule.IsActiveSchedule = IsActiveSchedule ?? schedule.IsActiveSchedule;
                schedule.IsSetSession     = IsSetSession ?? schedule.IsSetSession;
                schedule.SessionName      = SessionName ?? schedule.SessionName;
                schedule.HasExpiration    = HasExpiration ?? schedule.HasExpiration;
                schedule.StartDate        = startDate ?? schedule.StartDate;

                var result = await _dataStore.ActivitySchedule.Update(schedule);

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
                    IsActiveSchedule = result.Result.IsActiveSchedule,
                    IsSetSession = result.Result.IsSetSession,
                    SessionName = SessionName ?? schedule.SessionName,
                    HasExpiration = HasExpiration ?? schedule.HasExpiration,
                    StartDate     = startDate ?? schedule.StartDate,
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
