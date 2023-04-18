using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors.Results;

public class GenerateResponseResult 
{
    // 0  = no action, 1 = redirect
    public int Action {get; set;}
    public string Url {get; set;}
    public IEnumerable<MetaData> MetaDatas {get; set;}
}