using Cinnamon.Framework.ApiCommand.ApiData.ResetPassword.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ResetPassword.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IResetPasswordData 
{
    Task<AppResult<GetResetPasswordResult>> GetResetPasswordById(int id);
    Task<AppResult<GetResetPasswordResult>> GetResetPasswordByGuidToken(GetResetPasswordArgs args);
    Task<AppResult<GetAllResetPasswordResult>> GetAllResetPassword(GetAllResetPasswordArgs args);
    Task<AppResult<CreateResetPasswordResult>> CreateResetPassword(CreateResetPasswordArgs args);
    Task<AppResult<UpdateResetPasswordResult>> UpdateResetPassword(UpdateResetPasswordArgs args);
}