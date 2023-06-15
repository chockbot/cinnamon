namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.ApiCommand.ApiCore;

public class GetFavoritesByCustomerResult
{
    public IEnumerable<Favorite> Favorites { get; set; }

    public class Favorite
    {
        public int ActivityId { get; set; }
        public int CustomerId { get; set; }
    }
}