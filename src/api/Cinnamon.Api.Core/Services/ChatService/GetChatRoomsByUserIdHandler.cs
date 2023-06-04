using Cinnamon.Api.Core.Modules.DataAccess.AdminUser;
using Cinnamon.Api.Core.Modules.DataAccess.ChatHistory;
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
    public class GetChatRoomsByUserIdHandler : IGetChatRoomsByUserIdHandler
    {
        private readonly IChatRoomData chatRoomData;
        public GetChatRoomsByUserIdHandler(IChatRoomData chatRoomData)
        {
            this.chatRoomData = chatRoomData;
        }

        public AppResult<GetChatRoomsByUserIdResult> Execute(GetChatRoomsByUserIdArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetChatRoomsByUserIdResult>.CreateFailed(ex, "An error occured in GetChatRoomsByUserIdHandler");
            }
        }
        public async Task<AppResult<GetChatRoomsByUserIdResult>> ExecuteAsync(GetChatRoomsByUserIdArgs args)
        {
            try
            {
                var result = await chatRoomData.GetChatRoomsByUserId(new Framework.ApiCommand.ApiData.ChatRoom.Request.GetChatRoomsByUserIdArgs
                {
                    UserId = args.UserId
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<GetChatRoomsByUserIdResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<GetChatRoomsByUserIdResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetChatRoomsByUserIdHandler");
                }

                var chatResult = result.Result.Result;

                return AppResult<GetChatRoomsByUserIdResult>.CreateSucceeded(new GetChatRoomsByUserIdResult
                {
                    ChatRooms = chatResult.Select(c => new ChatRoom
                    {
                        ChatRoomId       = c.ChatRoomId,
                        DateCreated      = c.DateCreated,
                        FromFirstName    = c.FromFirstName,
                        FromLastName     = c.FromLastName,
                        FromUserId       = c.FromUserId,
                        Message          = c.Message,
                        ToFirstName      = c.ToFirstName,
                        ToLastName       = c.ToLastName,
                        ToUserId         = c.ToUserId,
                        ToProfilePath    = c.ToProfilePath,
                        FromProfilePath  = c.FromProfilePath,
                        ToConnectionId   = c.ToConnectionId,
                        FromConnectionId = c.FromConnectionId,
                        HasNewMessage    = c.HasNewMessage,
                        FromProfileLink  = c.FromProfileLink,
                    })
                }, "successfully called GetChatRoomsByUserIdHandler");

            }
            catch (Exception ex)
            {
                return AppResult<GetChatRoomsByUserIdResult>.CreateFailed(ex, "An error occured in GetChatRoomsByUserIdHandler");
            }
        }
    }
}
