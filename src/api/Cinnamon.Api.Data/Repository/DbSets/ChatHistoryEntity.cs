using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class ChatHistoryEntity : GenericEntity<ChatHistory>, IChatHistory
    {
        private readonly ApplicationContext applicationContext;

        public ChatHistoryEntity(ApplicationContext applicationContext)
            : base(applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<AppResult<IEnumerable<ChatHistory>>> GetOrderedChatHistoryByChatRoomId(Expression<Func<ChatHistory, bool>> expression,
        int? take = 100, int? skip = 0, IEnumerable<Expression<Func<ChatHistory, object>>>? includes = null)
        {
            try
            {
                int limitCount = take.HasValue ? take.Value : int.MaxValue;
                int skipCount = skip.HasValue ? skip.Value : 0;

                var query = applicationContext.Set<ChatHistory>().OrderByDescending(a => a.Id).Where(expression);

                query = query.Skip(skipCount).Take(limitCount);

                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                var results = query.AsQueryable();
                return AppResult<IEnumerable<ChatHistory>>.CreateSucceeded(results, "Successfully find entities");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ChatHistory>>.CreateFailed(ex, "An error occured when finding entities");
            }
        }
    }
}
