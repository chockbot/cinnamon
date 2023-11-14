using Cinnamon.Framework.ApiCommand.ApiCore.DTO.Customer;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Response;

public class ChangEmailAddressResult : AbstractModel<ChangeEmailDTO>
{
    public string Email { get; set; }
}
