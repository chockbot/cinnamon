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
    public class UpdateChatHistoryHandler : IUpdateChatHistoryHandler
    {
        private readonly IChatHistoryData chatHistoryData;

        public UpdateChatHistoryHandler(IChatHistoryData chatHistoryData)
        {
            this.chatHistoryData = chatHistoryData;
        }

        public AppResult<UpdateChatHistoryResult> Execute(UpdateChatHistoryArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<UpdateChatHistoryResult>.CreateFailed(ex, "An error occured in UpdateChatHistoryHandler");
            }
        }

        public async Task<AppResult<UpdateChatHistoryResult>> ExecuteAsync(UpdateChatHistoryArgs args)
        {
            try
            {
                var result = await chatHistoryData.UpdateChatHistory(new Framework.ApiCommand.ApiCore.ChatHistory.Request.UpdateChatHistoryArgs
                {
                    ChatRoomId = args.ChatRoomId,
                    FromUserId = args.FromUserId,
                    ToUserId   = args.ToUserId,
                    IsViewed   = args.IsViewed
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<UpdateChatHistoryResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<UpdateChatHistoryResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in UpdateChatHistoryHandler");
                }

                var chatResult = result.Result.Result;

                return AppResult<UpdateChatHistoryResult>.CreateSucceeded(new UpdateChatHistoryResult
                {
                    IsSuccess = true
                }, "successfully called UpdateChatHistoryHandler");

            }
            catch (Exception ex)
            {
                return AppResult<UpdateChatHistoryResult>.CreateFailed(ex, "An error occured in UpdateChatHistoryHandler");
            }
        }
    }
}
