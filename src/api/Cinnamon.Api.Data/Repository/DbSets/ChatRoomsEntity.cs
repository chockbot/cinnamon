using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class ChatRoomsEntity : GenericEntity<ChatRoom>, IChatRooms
    {
        private readonly ApplicationContext applicationContext;

        public ChatRoomsEntity(ApplicationContext applicationContext) : base(applicationContext)
        {
            this.applicationContext = applicationContext;
        }
    }
}
