using Newtonsoft.Json;

namespace Cinnamon.Framework.Providers;

public class DefaultJsonSerialization : IJsonSerializationProvider
{
    public object? Deserialize(string jsonString)
    {
        return JsonConvert.DeserializeObject(jsonString);
    }

    public TJsonObjectType? Deserialize<TJsonObjectType>(string jsonString)
    {
        return JsonConvert.DeserializeObject<TJsonObjectType>(jsonString);
    }

    public TJsonObjectType? Deserialize<TJsonObjectType>(string jsonString, JsonConverter converter)
    {
        return JsonConvert.DeserializeObject<TJsonObjectType>(jsonString, converter);
    }

    public string Serialize(object target)
    {
        return JsonConvert.SerializeObject(target);
    }

    public string Serialize(object target, JsonConverter converter)
    {
        return JsonConvert.SerializeObject(target, converter);
    }
}