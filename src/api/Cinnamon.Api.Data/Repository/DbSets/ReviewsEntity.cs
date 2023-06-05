using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;
public class ReviewsEntity: GenericEntity<Reviews>, IReviews
{
    public ReviewsEntity(ApplicationContext applicationContext)
        :base(applicationContext)
    { 
    }
}
