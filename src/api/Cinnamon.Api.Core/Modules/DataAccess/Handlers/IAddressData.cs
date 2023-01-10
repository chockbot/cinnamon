using Cinnamon.Framework.ApiCommand.ApiData.Address.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Address.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers
{
    public interface IAddressData
    {
        Task<AppResult<GetAddressResult>> GetAddressById(int id);
        Task<AppResult<GetAddressResult>> GetAddressByActivityId(int id);
        Task<AppResult<GetAllAddressResult>> GetAllAddress(GetAllAddressArgs args);
        Task<AppResult<CreateAddressResult>> CreateAddress(CreateAddressArgs args);
        Task<AppResult<UpdatedAddressResult>> UpdateAddress(UpdateAddressArgs args);
    }
}
