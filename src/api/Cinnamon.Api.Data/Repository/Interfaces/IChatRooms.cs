using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IChatRooms : IGenericEntity<ChatRoom>
{
    Task<AppResult<IEnumerable<ChatRoom>>> GetChatRoomsByUserId(Expression<Func<ChatRoom, bool>> expression, IEnumerable<Expression<Func<ChatRoom, object>>>? includes = null);
}