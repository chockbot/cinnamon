using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ChatHistory.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.ChatHistory;

public class ChatHistoryData : IChatHistoryData
{
    private readonly IFlurlClient flurlClient;
	public ChatHistoryData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateChatHistoryResult>> CreateChatHistory(CreateChatHistoryArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("ChatHistory/Create")
                .PostJsonAsync(args)
                .ReceiveJson<CreateChatHistoryResult>();

            return AppResult<CreateChatHistoryResult>.CreateSucceeded(result, "Successfully posted create chat history api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateChatHistoryResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateChatHistoryResult>.CreateFailed(ex, "An error occured when posting create chat history api");
        }
    }
}
