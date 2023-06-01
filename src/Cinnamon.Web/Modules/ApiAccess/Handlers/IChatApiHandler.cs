using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers
{
    public interface IChatApiHandler
    {
        Task<AppResult<CreateChatHistoryResult>> CreateChatHistory(CreateChatHistoryArgs args, string token);
    }
}
