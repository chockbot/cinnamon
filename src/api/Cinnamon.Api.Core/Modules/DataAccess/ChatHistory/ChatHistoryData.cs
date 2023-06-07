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
                .Request("Chat/Create")
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

    public async Task<AppResult<UpdateChatHistoryResult>> UpdateChatHistory(UpdateChatHistoryArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Chat/Update")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateChatHistoryResult>();

            return AppResult<UpdateChatHistoryResult>.CreateSucceeded(result, "Successfully posted update chat history api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateChatHistoryResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateChatHistoryResult>.CreateFailed(ex, "An error occured when posting update chat history api");
        }
    }

    public async Task<AppResult<GetChatHistoryByChatRoomIdResult>> GetChatHistoryByChatRoomId(GetChatHistoryByChatRoomIdArgs args)
    {
        try
        {
            var result = await flurlClient
                        .Request("Chat/ChatHistories")
                        .SetQueryParams(args)
                        .GetJsonAsync<GetChatHistoryByChatRoomIdResult>();

            return AppResult<GetChatHistoryByChatRoomIdResult>.CreateSucceeded(result, "Successfully posted get chat history api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetChatHistoryByChatRoomIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetChatHistoryByChatRoomIdResult>.CreateFailed(ex, "An error occured when posting get chat history api");
        }
    }
}
