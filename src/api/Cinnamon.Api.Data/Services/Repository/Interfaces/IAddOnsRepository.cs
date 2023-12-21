using Cinnamon.Framework.ApiCommand.ApiData.DTO.AddOns;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IAddOnsRepository
{
    Task<AppResult<AddOnsDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<AddOnsDTO>>> GetAllAsync();

    Task<AppResult<IEnumerable<AddOnsDTO>>> GetByActivityId(int ActivityId);
    Task<AppResult<AddOnsDTO>> UpdateAddOn(int? AddOnId, int? ActivityId, string? Name, decimal? Price, string? UnitPrice, string? Description, int? Order);
    Task<AppResult<AddOnsDTO>> CreateAddOn(int ActivityId, string Name, decimal Price, string UnitPrice, string Description, int Order);
    Task<AppResult<IEnumerable<AddOnsDTO>>> CreateAddOns(int ActivityId, IEnumerable<AddOnsDTO> addons);
    Task<AppResult<IEnumerable<AddOnsDTO>>> UpdateAddOns(IEnumerable<AddOnsDTO> addons);
    Task<AppResult<bool>> DeleteManyAddOns(IEnumerable<int> addonIds);
    Task<AppResult<bool>> DeleteAddOn(int addOnId);
}
