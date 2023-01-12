using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class FamilyMemberEntity : GenericEntity<FamilyMember>, IFamilyMember
{
    public FamilyMemberEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
    }
}