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
    public class CreateChatConnectionHandler : ICreateChatConnectionHandler
    {
        private readonly IChatRoomData chatRoomData;

        public CreateChatConnectionHandler(IChatRoomData chatRoomData)
        {
            this.chatRoomData = chatRoomData;
        }

        public AppResult<CreateChatConnectionResult> Execute(CreateChatConnectionArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<CreateChatConnectionResult>.CreateFailed(ex, "An error occured in CreateChatConnectionHandler");
            }
        }

        public async Task<AppResult<CreateChatConnectionResult>> ExecuteAsync(CreateChatConnectionArgs args)
        {
            try
            {
                var result = await chatRoomData.CreateChatConnection(new Framework.ApiCommand.ApiData.ChatConnection.Request.CreateChatConnectionArgs
                {
                    ConnectionId = args.ConnectionId,
                    CustomerId = args.CustomerId,
                    IsConnected = args.IsConnected,
                    UserAgent = args.UserAgent
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<CreateChatConnectionResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<CreateChatConnectionResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in CreateChatConnectionHandler");
                }

                var chatResult = result.Result.Result;

                return AppResult<CreateChatConnectionResult>.CreateSucceeded(new CreateChatConnectionResult
                {
                    IsSuccess = result.Succeeded
                }, "successfully called CreateChatConnectionHandler");

            }
            catch (Exception ex)
            {
                return AppResult<CreateChatConnectionResult>.CreateFailed(ex, "An error occured in CreateChatConnectionHandler");
            }
        }
    }
}
