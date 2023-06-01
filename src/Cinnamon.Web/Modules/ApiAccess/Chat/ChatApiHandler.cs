using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.AdminUser.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatHistory.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.ChatRoom.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Activity;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.Admin
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
    }
}
