using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IChatUnreadNotification : IGenericEntity<ChatUnreadNotification>
{
    Task<AppResult<IEnumerable<ChatUnreadNotification>>> GetUnreadMessages();
}