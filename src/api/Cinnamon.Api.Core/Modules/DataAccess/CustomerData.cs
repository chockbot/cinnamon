using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Config;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess;

public class CustomerData : ICustomerData 
{
    private readonly IFlurlClient flurlClient;

    public CustomerData(ApplicationConfig config, IFlurlClientFactory flurlClientFactory)
    {
        this.flurlClient = flurlClientFactory.Get(config.ApiDataUrl);
    }

    private async Task Aapp()
    {
        var res = await flurlClient.Request("/").PostJsonAsync(new {success = true}).ReceiveJson();
        foreach(var r in res.Result)
        {
            if(r.Success)
            {

            }
        }
    }
}