using Cinnamon.Framework.ApiCommand.ApiData.Location.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Location.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Student.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers
{
    public interface IRegionData
    {
        Task<AppResult<GetAllRegionResult>> GetAllRegions(GetAllRegionArgs args);
    }
}
