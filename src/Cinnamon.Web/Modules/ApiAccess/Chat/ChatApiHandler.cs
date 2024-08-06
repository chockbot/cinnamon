using Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatConnection.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatConnection.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Activity;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.Chat
{
    public class ChatApiHandler : IChatApiHandler
    {
        private readonly IFlurlClient flurlClient;
        private readonly ILogger _logger;

        public ChatApiHandler(IFlurlClientFactory flurlFac, Config.Config config, ILogger<ChatApiHandler> logger)
        {
            flurlClient = flurlFac.Get(config.ApiUrl);
            _logger = logger;
        }

        public async Task<AppResult<CreateChatHistoryResult>> CreateChatHistory(CreateChatHistoryArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Chat/Create")
                    .PostJsonAsync(args)
                    .ReceiveJson<CreateChatHistoryResult>();

                return AppResult<CreateChatHistoryResult>.CreateSucceeded(result, "Successfully called create chat history api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<CreateChatHistoryResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<CreateChatHistoryResult>.CreateFailed(ex, "An error occured when calling create chat history api");
            }
        }

        public async Task<AppResult<UpdateChatHistoryResult>> UpdateChatHistory(UpdateChatHistoryArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                   .WithOAuthBearerToken(token)
                   .Request("Chat/Update")
                   .PostJsonAsync(args)
                   .ReceiveJson<UpdateChatHistoryResult>();

                return AppResult<UpdateChatHistoryResult>.CreateSucceeded(result, "Successfully called update chat history api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<UpdateChatHistoryResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdateChatHistoryResult>.CreateFailed(ex, "An error occured when calling update chat history api");
            }
        }

        public async Task<AppResult<GetChatHistoryByChatRoomIdResult>> GetChatHistoryByChatRoomId(GetChatHistoryByChatRoomIdArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                     .WithOAuthBearerToken(token)
                     .Request("Chat/ByChatRoomId")
                     .SetQueryParams(args)
                     .GetJsonAsync<GetChatHistoryByChatRoomIdResult>();

                return AppResult<GetChatHistoryByChatRoomIdResult>.CreateSucceeded(result, "Successfully called get chat history api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<GetChatHistoryByChatRoomIdResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetChatHistoryByChatRoomIdResult>.CreateFailed(ex, "An error occured when calling get chat history api");
            }
        }

        public async Task<AppResult<CreateChatRoomResult>> CreateChatRoom(CreateChatRoomArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Chat/Room/Create")
                    .PostJsonAsync(args)
                    .ReceiveJson<CreateChatRoomResult>();

                return AppResult<CreateChatRoomResult>.CreateSucceeded(result, "Successfully called create chat room api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<CreateChatRoomResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<CreateChatRoomResult>.CreateFailed(ex, "An error occured when calling create chat room api");
            }
        }

        public async Task<AppResult<GetChatRoomsByUserIdResult>> GetChatRoomsByUserId(GetChatRoomsByUserIdArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                     .WithOAuthBearerToken(token)
                     .Request("Chat/ChatRooms")
                     .SetQueryParams(args)
                     .GetJsonAsync<GetChatRoomsByUserIdResult>();

                return AppResult<GetChatRoomsByUserIdResult>.CreateSucceeded(result, "Successfully called get chat room api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<GetChatRoomsByUserIdResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetChatRoomsByUserIdResult>.CreateFailed(ex, "An error occured when calling get chat room api");
            }
        }

        public async Task<AppResult<UpdateConnectionIdResult>> UpdateConnectionId(UpdateConnectionIdArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Chat/Connection/Update")
                    .PostJsonAsync(args)
                    .ReceiveJson<UpdateConnectionIdResult>();

                return AppResult<UpdateConnectionIdResult>.CreateSucceeded(result, "Successfully called update connection id api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<UpdateConnectionIdResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdateConnectionIdResult>.CreateFailed(ex, "An error occured when calling update connection id api");
            }
        }

        public async Task<AppResult<GetChatMembersByChatRoomIdResult>> GetChatMembersByChatRoomId(GetChatMembersByChatRoomIdArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                     .WithOAuthBearerToken(token)
                     .Request("Chat/ChatMembers")
                     .SetQueryParams(args)
                     .GetJsonAsync<GetChatMembersByChatRoomIdResult>();

                return AppResult<GetChatMembersByChatRoomIdResult>.CreateSucceeded(result, "Successfully called get chat members api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<GetChatMembersByChatRoomIdResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetChatMembersByChatRoomIdResult>.CreateFailed(ex, "An error occured when calling get chat members api");
            }
        }

        public async Task<AppResult<GetChatConnectionByCustomerResult>> GetChatConnectionByCustomer(GetChatConnectionByCustomerArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                     .WithOAuthBearerToken(token)
                     .Request("Chat/ChatConnections")
                     .SetQueryParams(args)
                     .GetJsonAsync<GetChatConnectionByCustomerResult>();

                return AppResult<GetChatConnectionByCustomerResult>.CreateSucceeded(result, "Successfully called get chat connections api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<GetChatConnectionByCustomerResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetChatConnectionByCustomerResult>.CreateFailed(ex, "An error occured when calling get chat connections api");
            }
        }

        public async Task<AppResult<RequestMessageResult>> RequestMessage(RequestMessageArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Chat/RequestMessage")
                    .PostJsonAsync(args)
                    .ReceiveJson<RequestMessageResult>();

                return AppResult<RequestMessageResult>.CreateSucceeded(result, "Successfully called request message api.");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<RequestMessageResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<RequestMessageResult>.CreateFailed(ex, "An error occured when calling request message api.");
            }
        }

        public async Task<AppResult<GetRequestMessageResult>> GetRequestMessage(GetRequestMessageArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                     .WithOAuthBearerToken(token)
                     .Request("Chat/GetRequestMessage")
                     .SetQueryParams(args)
                     .GetJsonAsync<GetRequestMessageResult>();

                return AppResult<GetRequestMessageResult>.CreateSucceeded(result, "Successfully called get chat request message api.");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<GetRequestMessageResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetRequestMessageResult>.CreateFailed(ex, "An error occured when calling get chat request message api.");
            }
        }

        public async Task<AppResult<UpdateChatRoomNameResult>> UpdateChatRoomName(UpdateChatRoomNameArgs args, string token)
        {
            try
            {
                var result = await flurlClient
                    .WithOAuthBearerToken(token)
                    .Request("Chat/UpdateChatRoomName")
                    .PostJsonAsync(args)
                    .ReceiveJson<UpdateChatRoomNameResult>();

                return AppResult<UpdateChatRoomNameResult>.CreateSucceeded(result, "Successfully called update chat room name api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<UpdateChatRoomNameResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdateChatRoomNameResult>.CreateFailed(ex, "An error occured when calling update chat room name api");
            }
        }
    }
}
