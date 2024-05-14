using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.DirectStudent;

public class DirectStudentRepository : IDirectStudentRepository
{
    private readonly IDataStore dataStore;

    public DirectStudentRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<IEnumerable<DirectStudentDTO>>> CreateDirectStudents(IEnumerable<DirectStudentDTO> directStudents)
    {
        try
        {
            var createDirectStudentRes = await dataStore.DirectStudentInfo.CreateDirectStudents(directStudents);
            if(!createDirectStudentRes.Succeeded || createDirectStudentRes.Result is null)
            {
                return AppResult<IEnumerable<DirectStudentDTO>>.CreateFailed(new ApplicationException(createDirectStudentRes.Message), createDirectStudentRes.Message);
            }

            return AppResult<IEnumerable<DirectStudentDTO>>.CreateSucceeded(createDirectStudentRes.Result, "Successfully create direct students.");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<DirectStudentDTO>>.CreateFailed(ex, "An error occured when creating direct students.");
        }
    }
}