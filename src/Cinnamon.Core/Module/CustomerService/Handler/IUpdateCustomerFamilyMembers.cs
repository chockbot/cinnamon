using Cinnamon.Core.Common;
using Cinnamon.Core.Interactor;
using Cinnamon.Core.Module.CustomerService.Interactors;
using Cinnamon.Core.Module.CustomerService.Interactors.Results;

namespace Cinnamon.Core.Module.CustomerService.Handler;

public interface  IUpdateCustomerFamilyMembers : IInteractorHandler<UpdateFamilyMember, AppResult<UpdateFamilyMemberResult>> 
{
}