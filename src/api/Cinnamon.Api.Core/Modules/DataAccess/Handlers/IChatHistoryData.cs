using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ChatHistory.Response;
using Cinnamon.Framework.Common;
namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IChatHistoryData
{
    Task<AppResult<CreateChatHistoryResult>> CreateChatHistory(CreateChatHistoryArgs args);
}
