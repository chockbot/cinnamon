using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class ChatMemberEntity : GenericEntity<ChatMember>, IChatMember
    {
        private readonly ApplicationContext applicationContext;

        public ChatMemberEntity(ApplicationContext applicationContext) : base(applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<AppResult<IEnumerable<ChatMember>>> GetChatRoomsByUserId(Expression<Func<ChatMember, bool>> expression, IEnumerable<Expression<Func<ChatMember, object>>>? includes = null)
        {
            try
            {
                var query = applicationContext.Set<ChatMember>().Where(expression);

                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                var results = query.ToList();

                return AppResult<IEnumerable<ChatMember>>.CreateSucceeded(results, "Successfully find entities");
            }
            catch (Exception ex)
            {
                return AppResult<IEnumerable<ChatMember>>.CreateFailed(ex, "An error occured when finding entities");
            }
        }
    }
}
