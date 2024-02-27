using Cinnamon.Framework.ApiCommand.ApiData.DTO.ChatUnreadNotification;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IChatUnreadNotificationRepository 
{
    Task<AppResult<ChatUnreadNotificationDTO>> CreateChatUnreadNotification(ChatUnreadNotificationDTO chatUnreadNotificationDto);
    Task<AppResult<IEnumerable<ChatUnreadNotificationDTO>>> GetUnreadMessages();
}