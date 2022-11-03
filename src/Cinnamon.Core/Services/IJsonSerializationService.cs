using Newtonsoft.Json;

namespace Cinnamon.Core.Services;

public interface IJsonSerializationService
{
    string Serialize(object target);
    string Serialize(object target, JsonConverter jsonConverter);
    object Deserialize(string jsonString);
    TJsonObjectType Deserialize<TJsonObjectType>(string jsonString);
    TJsonObjectType Deserialize<TJsonObjectType>(string jsonString, JsonConverter jsonConverter);
}