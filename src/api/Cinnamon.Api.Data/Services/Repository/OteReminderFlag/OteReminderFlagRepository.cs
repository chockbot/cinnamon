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

    public async Task<AppResult<OteReminderFlagDTO>> GetReminderFlag(int activityId, int dateId)
    {
        try
        {
            var result = await dataStore.OteReminderFlag.FindFirstAsync(r => r.ActivityId == activityId && r.OteDateId == dateId);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<OteReminderFlagDTO>.CreateFailed(new ApplicationException("Unable to locate reminder flag."), "Unable to locate reminder flag.");
            }

            var dto = mapper.Map<OteReminderFlagDTO>(result.Result);
            return AppResult<OteReminderFlagDTO>.CreateSucceeded(dto, "Successsfully create ote reminder.");
        }
        catch (Exception ex)
        {
            return AppResult<OteReminderFlagDTO>.CreateFailed(ex, "An error occured when creating ote reminder flag.");
        }
    }
}