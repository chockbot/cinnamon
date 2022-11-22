using Cinnamon.Core;
using Cinnamon.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinnamon.Data;

public class FamilyMember : BaseDbSet<FamilyMemberModel>, IFamilyMembers 
{
     public FamilyMember(DataStoreDbContext dbContext) : base(dbContext) { }
     protected override DbSet<FamilyMemberModel> Table => mDbContext.FamilyMembers;
}