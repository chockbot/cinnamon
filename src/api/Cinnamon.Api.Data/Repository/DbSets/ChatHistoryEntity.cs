using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class ChatHistoryEntity : GenericEntity<ChatHistory>, IChatHistory
    {
        public ChatHistoryEntity(ApplicationContext applicationContext) : base(applicationContext)
        {

        }
    }
}
