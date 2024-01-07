using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteDate;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.OteDate;

public class OteDateRepository : IOteDateRepository
{
    private readonly IDataStore dataStore;

    public OteDateRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }
    
    public async Task<AppResult<OteDateDTO>> GetOteDateById(int id)
    {
        try
        {
            var result = await dataStore.OteDate.GetByIdAsync(id);
            if(!result.Succeeded || result.Result is null)
            {
                return AppResult<OteDateDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            var oteDate = result.Result;

            return AppResult<OteDateDTO>.CreateSucceeded(
                new OteDateDTO {
                    Date = oteDate.Date,
                    DateEnd = oteDate.DateEnd,
                    DateStart = oteDate.DateStart,
                    Id = oteDate.Id,
                    ScheduleId = oteDate.OteScheduleId
                },
                "Successfully get ote date by id"
            );
        }
        catch (Exception ex)
        {
            return AppResult<OteDateDTO>.CreateFailed(ex, "An error occured when getting ote dates");
        }
    }
}