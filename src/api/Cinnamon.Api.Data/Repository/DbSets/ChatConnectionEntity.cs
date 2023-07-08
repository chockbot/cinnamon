using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class ChatConnectionEntity : GenericEntity<ChatConnection>, IChatConnection
    {
        private readonly ApplicationContext applicationContext;

        public ChatConnectionEntity(ApplicationContext applicationContext) : base(applicationContext)
        {
            this.applicationContext = applicationContext;
        }
    }
}
