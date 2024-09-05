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
    public class CreateChatRoomHandler : ICreateChatRoomHandler
    {
        private readonly IChatRoomData chatRoomData;

        public CreateChatRoomHandler(IChatRoomData chatRoomData)
        {
            this.chatRoomData = chatRoomData;
        }

        public AppResult<CreateChatRoomResult> Execute(CreateChatRoomArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<CreateChatRoomResult>.CreateFailed(ex, "An error occured in CreateChatRoomHandler");
            }
        }

        public async Task<AppResult<CreateChatRoomResult>> ExecuteAsync(CreateChatRoomArgs args)
        {
            try
            {
                // prevent user from creating chat room with themselves
                if(args.ToUserId == args.FromUserId)
                {
                    return AppResult<CreateChatRoomResult>.CreateFailed(new ApplicationException("ToUserId and FromUserId cannot be the same"), "ToUserId and FromUserId cannot be the same");
                }

                var result = await chatRoomData.CreateChatRoom(new Framework.ApiCommand.ApiData.ChatRoom.Request.CreateChatRoomArgs
                {
                    ToUserId  = args.ToUserId,
                    FromUserId = args.FromUserId,
                    ChatType = args.ChatType,
                    GroupName = args.GroupName,
                    ChatName = args.ChatName
                });

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<CreateChatRoomResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                if (result.Succeeded && !result.Result.IsSuccess)
                {
                    return AppResult<CreateChatRoomResult>.CreateFailed(
                        new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in CreateChatRoomHandler");
                }

                var chatResult = result.Result.Result;

                return AppResult<CreateChatRoomResult>.CreateSucceeded(new CreateChatRoomResult
                {
                    ChatRoomId = chatResult.ChatRoomId,
                    FromUserId = chatResult.FromUserId,
                    ToUserId   = chatResult.ToUserId,
                    GroupName  = chatResult.GroupName,
                }, "successfully called CreateChatRoomHandler");

            }
            catch (Exception ex)
            {
                return AppResult<CreateChatRoomResult>.CreateFailed(ex, "An error occured in CreateChatRoomHandler");
            }
        }
    }
}
