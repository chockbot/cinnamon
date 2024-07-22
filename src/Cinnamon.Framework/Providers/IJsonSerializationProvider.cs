using Newtonsoft.Json;

namespace Cinnamon.Framework.Providers;

public interface IJsonSerializationProvider 
{
    string Serialize(object target);
    string Serialize(object target, JsonConverter converter);
    object? Deserialize(string jsonString);
    TJsonObjectType? Deserialize<TJsonObjectType>(string jsonString);
    TJsonObjectType? Deserialize<TJsonObjectType>(string jsonString, JsonConverter converter);
}