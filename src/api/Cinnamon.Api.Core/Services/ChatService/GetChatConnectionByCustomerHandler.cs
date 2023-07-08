using Cinnamon.Api.Core.Modules.DataAccess.ChatHistory;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Interactors;
using Cinnamon.Api.Core.Services.ChatService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ChatService
{
    public class GetChatConnectionByCustomerHandler : IGetChatConnectionByCustomerHandler
    {
        private readonly IChatRoomData chatRoomData;
        public GetChatConnectionByCustomerHandler(IChatRoomData chatRoomData)
        {
            this.chatRoomData = chatRoomData;
        }
        public AppResult<GetChatConnectionByCustomerResult> Execute(GetChatConnectionByCustomerArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetChatConnectionByCustomerResult>.CreateFailed(ex, "An error occured in GetChatConnectionByCustomerHandler");
            }
        }

        public async Task<AppResult<GetChatConnectionByCustomerResult>> ExecuteAsync(GetChatConnectionByCustomerArgs args)
        {
            try
            {
                var result = await chatRoomData.GetChatConnectionByCustomer(new Framework.ApiCommand.ApiData.ChatConnection.Request.GetChatConnectionByCustomerArgs
                {
                    CustomerId = args.CustomerId
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<GetChatConnectionByCustomerResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<GetChatConnectionByCustomerResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetChatConnectionByCustomerHandler");
                }

                var chatResult = result.Result.Result;

                return AppResult<GetChatConnectionByCustomerResult>.CreateSucceeded(new GetChatConnectionByCustomerResult
                {
                    ChatConnections = chatResult.Select(c => new ChatConnection
                    {
                        ConnectionId = c.ConnectionId,
                        CustomerId = c.CustomerId,
                        IsConnected = c.IsConnected,
                        UserAgent = c.UserAgent
                    })
                }, "successfully called GetChatConnectionByCustomerHandler");

            }
            catch (Exception ex)
            {
                return AppResult<GetChatConnectionByCustomerResult>.CreateFailed(ex, "An error occured in GetChatConnectionByCustomerHandler");
            }
        }
    }
}
