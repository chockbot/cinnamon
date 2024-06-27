using System.Linq.Expressions;
using AutoMapper;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteReminderFlag;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.OteReminderFlag;

public class OteReminderFlagRepository : IOteReminderRepository
{
    private readonly IDataStore dataStore;
    private readonly IMapper mapper;

    public OteReminderFlagRepository(IDataStore dataStore, IMapper mapper)
    {
        this.dataStore = dataStore;
        this.mapper = mapper;
    }

    public async Task<AppResult<OteReminderFlagDTO>> CreateReminderFlag(OteReminderFlagDTO reminderFlag)
    {
        try
        {
            var entity = mapper.Map<Entities.OteReminderFlag>(reminderFlag);
            
            var createRes = await dataStore.OteReminderFlag.Add(entity);
            if(!createRes.Succeeded || createRes.Result is null)
            {
                return AppResult<OteReminderFlagDTO>.CreateFailed(new ApplicationException(createRes.Message), createRes.Message);
            }

            var dto = mapper.Map<OteReminderFlagDTO>(createRes.Result);
            return AppResult<OteReminderFlagDTO>.CreateSucceeded(dto, "Successsfully create ote reminder.");
        }
        catch (Exception ex)
        {
            return AppResult<OteReminderFlagDTO>.CreateFailed(ex, "An error occured when creating ote reminder flag.");
        }
    }

    public async Task<AppResult<IEnumerable<OteReminderFlagDTO>>> GetReminderFlags(int? activityId, int? dateId)
    {
        try
        {
            Expression<Func<Entities.OteReminderFlag, bool>> filter = r => 
                (activityId.HasValue ? r.ActivityId == activityId.Value : true) &&
                (dateId.HasValue ? r.OteDateId == dateId.Value : true);

            var result = await dataStore.OteReminderFlag.FindAsync(filter);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<OteReminderFlagDTO>>.CreateFailed(new ApplicationException("Unable to locate reminder flag."), "Unable to locate reminder flag.");
            }

            var dto = mapper.Map<IEnumerable<OteReminderFlagDTO>>(result.Result);
            return AppResult<IEnumerable<OteReminderFlagDTO>>.CreateSucceeded(dto, "Successsfully create ote reminder.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteReminderFlagDTO>>.CreateFailed(ex, "An error occured when creating ote reminder flag.");
        }
    }

    public async Task<AppResult<IEnumerable<OteForReminderDTO>>> GetEventsForReminder()
    {
        try
        {
            var result = await dataStore.OteReminderFlag.GetEventsForReminder();
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<OteForReminderDTO>>.CreateFailed(
                    new ApplicationException("Unable to get events for reminder."), "Unable to get events for reminder.");
            }

            return AppResult<IEnumerable<OteForReminderDTO>>.CreateSucceeded(result.Result, "Successsfully get events for reminder.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<OteForReminderDTO>>.CreateFailed(ex, "An error occured when getting events for reminder.");
        }
    }

    public async Task<AppResult<IEnumerable<CustomersNeedToRemindDTO>>> CustomersToRemind(int activityId, int oteDateId)
    {
        try
        {
            var result = await dataStore.OteReminderFlag.CustomersToRemind(activityId, oteDateId);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<IEnumerable<CustomersNeedToRemindDTO>>.CreateFailed(
                    new ApplicationException("Unable to get customers for reminder."), "Unable to get customers for reminder.");
            }

            return AppResult<IEnumerable<CustomersNeedToRemindDTO>>.CreateSucceeded(result.Result, "Successsfully get customers for reminder.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<CustomersNeedToRemindDTO>>.CreateFailed(ex, "An error occured when getting customers for reminder.");
        }
    }
}