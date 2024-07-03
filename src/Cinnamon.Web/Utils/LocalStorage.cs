namespace Cinnamon.Web.Utils;

public interface ILocalStorage 
{
    Task GetItem<T>(string key);
    Task SetItem(string key, object value);
    Task RemoveItem(string key);
}

public class LocalStorage : ILocalStorage
{
    public Task GetItem<T>(string key)
    {
        throw new NotImplementedException();
    }

    public Task RemoveItem(string key)
    {
        throw new NotImplementedException();
    }

    public Task SetItem(string key, object value)
    {
        throw new NotImplementedException();
    }
}