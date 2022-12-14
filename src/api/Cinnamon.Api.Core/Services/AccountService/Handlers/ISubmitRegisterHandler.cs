using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Customer.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService.Handlers;

public interface ISubmitRegisterHandler 
{
    AppResult<SubmitRegisterResult> Execute(SubmitRegisterArgs args);
    Task<AppResult<SubmitRegisterResult>> ExecuteAsync(SubmitRegisterArgs args);
}