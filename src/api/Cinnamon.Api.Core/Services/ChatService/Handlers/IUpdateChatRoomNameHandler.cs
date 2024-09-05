using Cinnamon.Api.Core.Services.ChatService.Interactors;
using Cinnamon.Api.Core.Services.ChatService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ChatService.Handlers;

public interface IUpdateChatRoomNameHandler : IInteractorHandler<UpdateChatRoomNameArgs, AppResult<UpdateChatRoomNameResult>>
{
}