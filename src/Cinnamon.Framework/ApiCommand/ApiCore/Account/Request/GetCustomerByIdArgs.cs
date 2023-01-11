using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class GetCustomerByIdArgs
{
    [Required]
    public int Id { get; set; }
}
