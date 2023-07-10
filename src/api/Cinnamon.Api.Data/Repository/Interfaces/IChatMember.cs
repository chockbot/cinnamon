using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IChatMember : IGenericEntity<ChatMember>
{
    Task<AppResult<IEnumerable<ChatMember>>> GetChatRoomsByUserId(Expression<Func<ChatMember, bool>> expression, IEnumerable<Expression<Func<ChatMember, object>>>? includes = null);
}