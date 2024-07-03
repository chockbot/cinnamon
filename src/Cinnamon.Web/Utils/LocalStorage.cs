using Cinnamon.Framework.Providers;
using Microsoft.JSInterop;

namespace Cinnamon.Web.Utils;

public interface ILocalStorage 
{
    Task<T?> GetItem<T>(string key);
    Task SetItem(string key, object value);
    Task RemoveItem(string key);
}

public class LocalStorage : ILocalStorage
{
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IJSRuntime jsRuntime;

    public LocalStorage(IJsonSerializationProvider jsonSerializationProvider, IJSRuntime jsRuntime)
    {
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.jsRuntime = jsRuntime;
    }
    
    public async Task<T?> GetItem<T>(string key)
    {
        var value = await jsRuntime.InvokeAsync<string>("MyLib.Utils.localStorage.getItem",key);
        return jsonSerializationProvider.Deserialize<T>(value);
    }

    public async Task RemoveItem(string key)
    {
        await jsRuntime.InvokeVoidAsync("MyLib.Utils.localStorage.removeItem", key);
    }

    public async Task SetItem(string key, object value)
    {
        await jsRuntime.InvokeVoidAsync("MyLib.Utils.localStorage.setItem", key, value);
    }
}