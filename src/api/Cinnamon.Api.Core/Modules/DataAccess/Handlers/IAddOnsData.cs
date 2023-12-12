using Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request;
using Cinnamon.Framework.ApiCommand.ApiData.AddOns.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;
public interface IAddOnsData
{
    Task<AppResult<GetAddOnResult>> GetAddOnById(int id);
    Task<AppResult<GetAllAddOnResult>> GetAllAddOns();
    Task<AppResult<CreateAddOnResult>> CreateAddOn(CreateAddOnArgs args);
    Task<AppResult<UpdateAddOnResult>> UpdateAddOn(UpdateAddOnArgs args);
    Task<AppResult<CreateAddOnsResult>> CreateManyAddOns(CreateAddOnsArgs args);
    Task<AppResult<UpdateAddOnsResult>> UpdateManyAddOns(UpdateAddOnsArgs args);
    Task<AppResult<DeleteAddOnsResult>> DeleteManyAddOns(DeleteAddOnsArgs args);
}
