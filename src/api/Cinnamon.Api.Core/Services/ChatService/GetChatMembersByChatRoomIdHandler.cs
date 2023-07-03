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
    public class GetChatMembersByChatRoomIdHandler : IGetChatMembersByChatRoomIdHandler
    {
        private readonly IChatRoomData chatRoomData;

        public GetChatMembersByChatRoomIdHandler(IChatRoomData chatRoomData)
        {
            this.chatRoomData = chatRoomData;
        }

        public AppResult<GetChatMembersByChatRoomIdResult> Execute(GetChatMembersByChatRoomIdArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetChatMembersByChatRoomIdResult>.CreateFailed(ex, "An error occured in GetChatMembersByChatRoomIdHandler");
            }
        }

        public async Task<AppResult<GetChatMembersByChatRoomIdResult>> ExecuteAsync(GetChatMembersByChatRoomIdArgs args)
        {
            try
            {
                var result = await chatRoomData.GetChatMembersByChatRoomId(new Framework.ApiCommand.ApiData.ChatRoom.Request.GetChatMembersByChatRoomIdArgs
                {
                    ChatRoomId = args.ChatRoomId,
                    UserId = args.UserId
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<GetChatMembersByChatRoomIdResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<GetChatMembersByChatRoomIdResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetChatMembersByChatRoomIdHandler");
                }

                var chatResult = result.Result.Result;

                return AppResult<GetChatMembersByChatRoomIdResult>.CreateSucceeded(new GetChatMembersByChatRoomIdResult
                {
                    ChatMembers = chatResult.Select(c => new ChatMembers
                    {
                        FromFirstName = c.FromFirstName,
                        FromLastName = c.FromLastName,
                        FromProfilePath = c.FromProfilePath,
                        FromProfileLink = c.FromProfileLink,
                        FromUserId = c.FromUserId,
                        ChatMemberType = c.ChatMemberType,
                        CommonPrivateChatRoomId = c.CommonPrivateChatRoomId
                    })
                }, "successfully called GetChatMembersByChatRoomIdHandler");

            }
            catch (Exception ex)
            {
                return AppResult<GetChatMembersByChatRoomIdResult>.CreateFailed(ex, "An error occured in GetChatMembersByChatRoomIdHandler");
            }
        }
    }
}
