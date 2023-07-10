using Cinnamon.Framework.ApiCommand.ApiCore.DTO.FamilyMember;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Account.Request;

public class VerifyUserNotificationArgs
{
    public string Email { get; set; }
    public string IdAttached { get; set; }
    public string BankDetails { get; set; }
}