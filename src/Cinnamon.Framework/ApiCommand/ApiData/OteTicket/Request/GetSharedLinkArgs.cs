namespace Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;

public class GetSharedLinkArgs
{
    public int? ActivityId {get; set;}

    public int? OteDateId {get; set;}

    public string? Guid {get; set;}

    public string? Token {get; set;}
}