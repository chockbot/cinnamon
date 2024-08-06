using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Interactors;
using Cinnamon.Api.Core.Services.ChatService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ChatService;

public class UpdateChatRoomNameHandler : IUpdateChatRoomNameHandler
{
    private readonly IGetProfileHandler getProfileHandler;

    public UpdateChatRoomNameHandler(IGetProfileHandler getProfileHandler)
    {
        this.getProfileHandler = getProfileHandler;
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
        }
        catch (Exception ex)
        {
            return AppResult<UpdateChatRoomNameResult>.CreateFailed(ex, "An error occured when updating chat room name");
        }
    }
}