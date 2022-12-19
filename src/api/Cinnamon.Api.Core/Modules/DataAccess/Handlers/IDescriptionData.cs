using Cinnamon.Framework.ApiCommand.ApiData.Address.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Address.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Description.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Description.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers
{
    public interface IDescriptionData
    {
        Task<AppResult<GetDescriptionResult>> GetDescriptionById(int id);
        Task<AppResult<GetAllDescriptionResult>> GetAllDescription();
        Task<AppResult<CreateDescriptionResult>> CreateDescription(CreateDescriptionArgs args);
        Task<AppResult<UpdatedDescriptionResult>> UpdateDescription(UpdateDescriptionArgs args);
    }
}
