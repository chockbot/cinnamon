using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class SearchTagsEntity : GenericEntity<SearchTags>, ISearchTags
{
    public SearchTagsEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}