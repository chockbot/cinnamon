using Cinnamon.Api.Data.Services.Repository.Activity.DTO;
using Cinnamon.Api.Data.Services.Repository.ActivityAddress.DTO;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces
{
    public interface IAddressRepository
    {
        Task<AppResult<AddressDTO>> GetByIdAsync(int id);
        Task<AppResult<IEnumerable<AddressDTO>>> GetAllAsync();
        Task<AppResult<AddressDTO>> UpdateAddress(int AddressId, string Address1, string Address2, string District, string City);
        Task<AppResult<AddressDTO>> RemoveAddress(int AddressId);
        Task<AppResult<AddressDTO>> CreateAddress(int ActivityId, string Address1, string Address2, string District, string City);
    }
}
