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
    public class UpdateChatMemberHandler : IUpdateChatMemberHandler
    {
        private readonly IChatRoomData chatRoomData;

        public UpdateChatMemberHandler(IChatRoomData chatRoomData)
        {
            this.chatRoomData = chatRoomData;
        }

        public AppResult<UpdateChatMemberResult> Execute(UpdateChatMemberArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<UpdateChatMemberResult>.CreateFailed(ex, "An error occured in UpdateChatMemberHandler");
            }
        }

        public async Task<AppResult<UpdateChatMemberResult>> ExecuteAsync(UpdateChatMemberArgs args)
        {
            try
            {
                var result = await chatRoomData.UpdateChatMember(new Framework.ApiCommand.ApiData.ChatRoom.Request.UpdateChatMemberArgs
                {
                    ChatRoomId = args.ChatRoomId,
                    UserId = args.UserId,
                    HasLeft = args.HasLeft
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<UpdateChatMemberResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<UpdateChatMemberResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in UpdateChatMemberHandler");
                }

                return AppResult<UpdateChatMemberResult>.CreateSucceeded(new UpdateChatMemberResult
                {
                    IsSuccess = result.Result.IsSuccess
                }, "successfully called UpdateChatMemberHandler");

            }
            catch (Exception ex)
            {
                return AppResult<UpdateChatMemberResult>.CreateFailed(ex, "An error occured in UpdateChatMemberHandler");
            }
        }
    }
}
