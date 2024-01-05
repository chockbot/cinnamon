using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteDate;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IOteDateRepository 
{
    Task<AppResult<OteDateDTO>> GetOteDateById(int id);
}