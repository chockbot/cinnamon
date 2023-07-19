using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets
{
    public class FavoriteEntity : GenericEntity<Favorite>, IFavorite
    {
        private readonly ApplicationContext applicationContext;

        public FavoriteEntity(ApplicationContext applicationContext) : base(applicationContext)
        {
            this.applicationContext = applicationContext;
        }
    }
}
