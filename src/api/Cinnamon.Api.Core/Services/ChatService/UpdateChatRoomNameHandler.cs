using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Interactors;
using Cinnamon.Api.Core.Services.ChatService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ChatService;

public class UpdateChatRoomNameHandler : IUpdateChatRoomNameHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IChatRoomData chatRoomData;

    public UpdateChatRoomNameHandler(IGetProfileHandler getProfileHandler, IChatRoomData chatRoomData)
    {
        this.getProfileHandler = getProfileHandler;
        this.chatRoomData = chatRoomData;
    }
    
    public AppResult<UpdateChatRoomNameResult> Execute(UpdateChatRoomNameArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<UpdateChatRoomNameResult>> ExecuteAsync(UpdateChatRoomNameArgs args)
    {
        try
        {
            var currentProfile = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!currentProfile.Succeeded || currentProfile.Result is null)
            {
                return AppResult<UpdateChatRoomNameResult>.CreateFailed(new ApplicationException(currentProfile.Message),"Failed to get current profile");
            }
            var profile = currentProfile.Result;

            var chatMembers = await chatRoomData.GetChatRoomMembers(new Framework.ApiCommand.ApiData.ChatConnection.Request.GetChatMemberArgs {
                ChatRoomId = args.ChatRoomId,
                UserId = profile.Id
            });
            if(!chatMembers.Succeeded || chatMembers.Result is null || !chatMembers.Result.IsSuccess)
            {
                return AppResult<UpdateChatRoomNameResult>.CreateFailed(new ApplicationException(chatMembers.Message),"Failed to get chat members");
            }

            if(chatMembers.Result.Result.Count() == 0)
            {
                return AppResult<UpdateChatRoomNameResult>.CreateFailed(
                    new ApplicationException("User is not a member of the chat room"),"User is not a member of the chat room");
            }

            bool isOwner = chatMembers.Result.Result.Any(c => c.ChatMemberType == 2);

            if(!isOwner)
            {
                return AppResult<UpdateChatRoomNameResult>.CreateFailed(
                    new ApplicationException("User is not the owner of the chat room"),"User is not the owner of the chat room");
            }

            var updateChatRoomNameRes = await chatRoomData.UpdateChatRoomName(new Framework.ApiCommand.ApiData.ChatConnection.Request.UpdateChatRoomNameArgs {
                ChatRoomId = args.ChatRoomId,
                NewChatRoomName = args.ChatRoomName
            });
            if(!updateChatRoomNameRes.Succeeded || updateChatRoomNameRes.Result is null || !updateChatRoomNameRes.Result.IsSuccess)
            {
                return AppResult<UpdateChatRoomNameResult>.CreateFailed(new ApplicationException(updateChatRoomNameRes.Message),"Failed to update chat room name");
            }

            return AppResult<UpdateChatRoomNameResult>.CreateSucceeded(new UpdateChatRoomNameResult {
                IsSuccess = true
            }, "Chat room name updated successfully");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateChatRoomNameResult>.CreateFailed(ex, "An error occured when updating chat room name");
        }
    }
}