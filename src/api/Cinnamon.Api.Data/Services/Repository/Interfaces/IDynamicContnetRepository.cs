using Cinnamon.Framework.ApiCommand.ApiData.DTO.DynamicContent;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IDynamicContnetRepository 
{
    Task<AppResult<DynamicContentDTO>> CreateDynamicContent(DynamicContentDTO args);
    Task<AppResult<DynamicContentDTO>> UpdateDynamicContent(DynamicContentDTO args);
    Task<AppResult<DynamicContentDTO>> GetDynamicContent(string identifier);
}