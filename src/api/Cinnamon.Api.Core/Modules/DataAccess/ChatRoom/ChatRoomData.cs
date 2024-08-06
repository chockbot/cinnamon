using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData.ChatConnection.Request;
using Cinnamon.Framework.ApiCommand.ApiData.ChatConnection.Response;
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

    public async Task<AppResult<CreateChatConnectionResult>> CreateChatConnection(CreateChatConnectionArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Chat/ChatConnection")
                .PostJsonAsync(args)
                .ReceiveJson<CreateChatConnectionResult>();

            return AppResult<CreateChatConnectionResult>.CreateSucceeded(result, "Successfully posted create chat connection api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateChatConnectionResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateChatConnectionResult>.CreateFailed(ex, "An error occured when posting create chat connection api");
        }
    }

    public async Task<AppResult<UpdateChatConnectionResult>> UpdateChatConnection(UpdateChatConnectionArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Chat/ChatConnection/Update")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateChatConnectionResult>();

            return AppResult<UpdateChatConnectionResult>.CreateSucceeded(result, "Successfully posted update chat connection api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateChatConnectionResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateChatConnectionResult>.CreateFailed(ex, "An error occured when posting update chat connection api");
        }
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

    public async Task<AppResult<GetChatMembersByChatRoomIdResult>> GetChatMembersByChatRoomId(GetChatMembersByChatRoomIdArgs args)
    {
        try
        {
            var result = await flurlClient
                        .Request("Chat/ChatMembers")
                        .SetQueryParams(args)
                        .GetJsonAsync<GetChatMembersByChatRoomIdResult>();

            return AppResult<GetChatMembersByChatRoomIdResult>.CreateSucceeded(result, "Successfully posted get chat members api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetChatMembersByChatRoomIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetChatMembersByChatRoomIdResult>.CreateFailed(ex, "An error occured when posting get chat members api");
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

    public async Task<AppResult<UpdateChatMemberResult>> UpdateChatMember(UpdateChatMemberArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Chat/ChatMember/Update")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateChatMemberResult>();

            return AppResult<UpdateChatMemberResult>.CreateSucceeded(result, "Successfully posted update chat member api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateChatMemberResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateChatMemberResult>.CreateFailed(ex, "An error occured when posting update chat member api");
        }
    }

    public async Task<AppResult<GetChatConnectionByCustomerResult>> GetChatConnectionByCustomer(GetChatConnectionByCustomerArgs args)
    {
        try
        {
            var result = await flurlClient
                        .Request("Chat/ChatConnections")
                        .SetQueryParams(args)
                        .GetJsonAsync<GetChatConnectionByCustomerResult>();

            return AppResult<GetChatConnectionByCustomerResult>.CreateSucceeded(result, "Successfully posted get chat connections api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetChatConnectionByCustomerResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetChatConnectionByCustomerResult>.CreateFailed(ex, "An error occured when posting get chat connections api");
        }
    }

    public async Task<AppResult<UpdateChatRoomNameResult>> UpdateChatRoomName(UpdateChatRoomNameArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("Chat/Room/UpdateName")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateChatRoomNameResult>();

            return AppResult<UpdateChatRoomNameResult>.CreateSucceeded(result, "Successfully posted update chat room api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateChatRoomNameResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateChatRoomNameResult>.CreateFailed(ex, "An error occured when posting update chat room api");
        }
    }
}
