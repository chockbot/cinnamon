using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class ChatRoomsEntity : GenericEntity<ChatRoom>, IChatRooms
    {
        public ChatRoomsEntity(ApplicationContext applicationContext) : base(applicationContext)
        {

        }
    }
}
