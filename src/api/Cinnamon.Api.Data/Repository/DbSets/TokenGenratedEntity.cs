using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class TokenGenratedEntity : GenericEntity<TokenGenerated>, ITokenGenerated 
{
    public TokenGenratedEntity(ApplicationContext applicationContext)
		:base(applicationContext)	
	{
	}
}