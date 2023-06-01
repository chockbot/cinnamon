using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ChatHistory.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ChatRoom.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatRoom.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.ChatRoom;

public class ChatRoomData : IChatRoomData
{
    private readonly IFlurlClient flurlClient;
	public ChatRoomData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateChatRoomResult>> CreateChatRoom(CreateChatRoomArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Chat/Room/Create")
                .PostJsonAsync(args)
                .ReceiveJson<CreateChatRoomResult>();

            return AppResult<CreateChatRoomResult>.CreateSucceeded(result, "Successfully posted create chat room api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateChatRoomResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateChatRoomResult>.CreateFailed(ex, "An error occured when posting create chat room api");
        }
    }

    public async Task<AppResult<GetChatRoomsByUserIdResult>> GetChatRoomsByUserId(GetChatRoomsByUserIdArgs args)
    {
        try
        {
            var result = await flurlClient
                        .Request("Chat/ChatRooms")
                        .SetQueryParams(args)
                        .GetJsonAsync<GetChatRoomsByUserIdResult>();

            return AppResult<GetChatRoomsByUserIdResult>.CreateSucceeded(result, "Successfully posted get chat room api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetChatRoomsByUserIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetChatRoomsByUserIdResult>.CreateFailed(ex, "An error occured when posting get chat room api");
        }
    }
}
