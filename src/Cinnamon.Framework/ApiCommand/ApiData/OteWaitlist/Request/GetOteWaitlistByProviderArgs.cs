namespace Cinnamon.Framework.ApiCommand.ApiData.OteWaitlist.Request;

public class GetOteWaitlistByProviderArgs
{
    public int ProviderId { get; set; }
    public int ActivityId { get; set; }
    public IEnumerable<int>? Status { get; set; }
}
