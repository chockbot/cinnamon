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
    public class CreateChatHistoryHandler : ICreateChatHistoryHandler
    {
        private readonly IChatHistoryData chatHistoryData;

        public CreateChatHistoryHandler(IChatHistoryData chatHistoryData)
        {
            this.chatHistoryData = chatHistoryData;
        }

        public AppResult<CreateChatHistoryResult> Execute(CreateChatHistoryArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<CreateChatHistoryResult>.CreateFailed(ex, "An error occured in CreateChatHistoryHandler");
            }
        }

        public async Task<AppResult<CreateChatHistoryResult>> ExecuteAsync(CreateChatHistoryArgs args)
        {
            try
            {
                var result = await chatHistoryData.CreateChatHistory(new Framework.ApiCommand.ApiCore.ChatHistory.Request.CreateChatHistoryArgs
                {
                    ChatRoomId       = args.ChatRoomId,
                    FromConnectionId = args.FromConnectionId,
                    FromUserId       = args.FromUserId,
                    IsViewed         = args.IsViewed,
                    Message          = args.Message,
                    ToConnectionId   = args.ToConnectionId,
                    ToUserId         = args.ToUserId,
                    ChatHistoryType  = args.ChatHistoryType
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<CreateChatHistoryResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<CreateChatHistoryResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in CreateChatHistoryHandler");
                }

                var chatResult = result.Result.Result;

                return AppResult<CreateChatHistoryResult>.CreateSucceeded(new CreateChatHistoryResult
                {
                    ChatRoomId       = chatResult.ChatRoomId,
                    FromConnectionId = chatResult.FromConnectionId,
                    FromUserId       = chatResult.FromUserId,
                    IsViewed         = chatResult.IsViewed,
                    Message          = chatResult.Message,
                    ToConnectionId   = chatResult.ToConnectionId,
                    ToUserId         = chatResult.ToUserId,
                }, "successfully called CreateChatHistoryHandler");

            }
            catch (Exception ex)
            {
                return AppResult<CreateChatHistoryResult>.CreateFailed(ex, "An error occured in CreateChatHistoryHandler");
            }
        }
    }
}
