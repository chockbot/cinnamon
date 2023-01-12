namespace Cinnamon.Api.Data.Extensions;

public static class ObjectExtenstion
{
    public static bool HasProperty(this Object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName) != null;
    }

    public static object? GetProperty(this Object obj, string propertyName)
    {
        return obj.GetType()?.GetProperty(propertyName)?.GetValue(obj);
    }
}