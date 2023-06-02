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

        public async Task<AppResult<IEnumerable<ChatRoom>>> GetChatRoomsByUserId(Expression<Func<ChatRoom, bool>> expression, IEnumerable<Expression<Func<ChatRoom, object>>>? includes = null)
        {
            try
            {
                var query = applicationContext.Set<ChatRoom>().Where(expression);

                if (includes != null)
                {
                   foreach (var include in includes)
                   {
                       query = query.Include(include);
                   }
                }

                var results = query.ToList();

                return AppResult<IEnumerable<ChatRoom>>.CreateSucceeded(results, "Successfully find entities");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ChatRoom>>.CreateFailed(ex, "An error occured when finding entities");
            }
        }
    }
}
