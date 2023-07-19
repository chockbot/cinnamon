using Cinnamon.Api.Core.Modules.DataAccess.AdminUser;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Interactors;
using Cinnamon.Api.Core.Services.ChatService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ChatService
{
    public class UpdateChatConnectionHandler : IUpdateChatConnectionHandler
    {
        private readonly IChatRoomData chatRoomData;

        public UpdateChatConnectionHandler(IChatRoomData chatRoomData)
        {
            this.chatRoomData = chatRoomData;
        }

        public AppResult<UpdateChatConnectionResult> Execute(UpdateChatConnectionArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<UpdateChatConnectionResult>.CreateFailed(ex, "An error occured in UpdateChatConnectionHandler");
            }
        }

        public async Task<AppResult<UpdateChatConnectionResult>> ExecuteAsync(UpdateChatConnectionArgs args)
        {
            try
            {
                var result = await chatRoomData.UpdateChatConnection(new Framework.ApiCommand.ApiData.ChatConnection.Request.UpdateChatConnectionArgs
                {
                    ConnectionId = args.ConnectionId,
                    CustomerId = args.CustomerId,
                    IsConnected = args.IsConnected,
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<UpdateChatConnectionResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<UpdateChatConnectionResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in UpdateChatConnectionHandler");
                }

                var chatResult = result.Result.Result;

                return AppResult<UpdateChatConnectionResult>.CreateSucceeded(new UpdateChatConnectionResult
                {
                    IsSuccess = result.Succeeded
                }, "successfully called UpdateChatConnectionHandler");

            }
            catch (Exception ex)
            {
                return AppResult<UpdateChatConnectionResult>.CreateFailed(ex, "An error occured in UpdateChatConnectionHandler");
            }
        }
    }
}
