using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

 public class SubCategoryEntity: GenericEntity<SubCategory>, ISubCategory
 {
	public SubCategoryEntity(ApplicationContext applicationContext)
		:base(applicationContext)	
	{
	}
 }

