using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IChatHistory : IGenericEntity<ChatHistory>
{
    Task<AppResult<IEnumerable<ChatHistory>>> GetOrderedChatHistoryByChatRoomId(Expression<Func<ChatHistory, bool>> expression,
        int? take = 100, int? skip = 0, IEnumerable<Expression<Func<ChatHistory, object>>>? includes = null);
}