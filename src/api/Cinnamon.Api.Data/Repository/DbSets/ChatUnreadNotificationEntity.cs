using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ChatUnreadNotificationEntity : GenericEntity<ChatUnreadNotification>, IChatUnreadNotification
{
    public ChatUnreadNotificationEntity(ApplicationContext applicationContext) : base(applicationContext)
    {
    }
}