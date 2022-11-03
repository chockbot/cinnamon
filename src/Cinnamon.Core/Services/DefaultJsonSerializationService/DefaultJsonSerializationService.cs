using Newtonsoft.Json;

namespace Cinnamon.Core.Services.DefaultJsonSerialization;

public class DefaultJsonSerializationService : IJsonSerializationService 
{
    public object Deserialize(string jsonString)
    {
        return JsonConvert.DeserializeObject(jsonString);
    }

    public TJsonObjectType Deserialize<TJsonObjectType>(string jsonString)
    {
        return JsonConvert.DeserializeObject<TJsonObjectType>(jsonString);
    }

    public TJsonObjectType Deserialize<TJsonObjectType>(string jsonString, JsonConverter jsonConverter)
    {
        return JsonConvert.DeserializeObject<TJsonObjectType>(jsonString, jsonConverter);
    }

    public string Serialize(object target)
    {
        return JsonConvert.SerializeObject(target);
    }

    public string Serialize(object target, JsonConverter jsonConverter)
    {
        return JsonConvert.SerializeObject(target, jsonConverter);
    }
}