using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class GetRequestedRefundsArgs
{
    [Required]
    public int Status {get; set;}
}